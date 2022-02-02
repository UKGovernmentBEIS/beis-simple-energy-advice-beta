
########################
# Start of configuration

# Name - This is the bit after 'sea-alpha-' - e.g. for sea-alpha-DEV, PAAS_ENV_SHORTNAME would just be 'DEV'
echo "What name do you want to give to the new environment? (just the part after the 'sea-alpha-' prefix please)"
read PAAS_ENV_SHORTNAME

# Database size - we normally use 'small-13' for testing and 'small-ha-13' for production
DATABSE_SIZE="small-13"

# App scale settings
APP_INSTANCES=1     # We use 2 for most environments, 3 for production
APP_DISK="1G"       # Not sure what this default should be - we'll have to test this and set a default later
APP_MEMORY="1G"     # In PaaS this is 4G for all environments other than production, which is 8GB


#################
# Start of script

#------
# Login
./LoginToGovPaas.sh


#-----------------
# Create the space
cf create-space "sea-alpha-${PAAS_ENV_SHORTNAME}" -o "beis-domestic-energy-advice-service"

# - Target future commands at this space
cf target -s "sea-alpha--${PAAS_ENV_SHORTNAME}"


#---------------------------
# Add AWS S3 backing service
# - Create the service
cf create-service aws-s3-bucket default "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage"

# - Create a key to access the service
#   Note: this is only needed to access the S3 bucket from outside of Gov.UK PaaS (e.g. from Azure)
cf create-service-key "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage" "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage-key" -c '{"allow_external_access": true}'

# - Get the key (and print to the console)
cf service-key "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage" "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage-key"


#-----------------------------------------
# Create Postgres database backing service
# - Create the database itself
cf create-service postgres "${DATABSE_SIZE}" "sea-alpha-${PAAS_ENV_SHORTNAME}-db"

echo "Waiting for the Postgres database to be created (normally takes 5-10 mins)"

RESULT=$(cf service "sea-alpha-${PAAS_ENV_SHORTNAME}-db")
while [ -z "${RESULT##*create in progress*}" ]
do
	echo -n "."
	sleep 5s
	RESULT=$(cf service "sea-alpha-${PAAS_ENV_SHORTNAME}-db")
done
echo "" # To create a new-line

if [ -z "${RESULT##*create succeeded*}" ]
then
	echo "Create successful"
	cf service "sea-alpha-${PAAS_ENV_SHORTNAME}-db"
else
	echo "Create failed"
	cf service "sea-alpha-${PAAS_ENV_SHORTNAME}-db"
	exit 1
fi

# - Create a key to access the service
#   Note: this is only needed to access the database from outside of Gov.UK PaaS (e.g. from a developer machine)
cf create-service-key "sea-alpha-${PAAS_ENV_SHORTNAME}-db" "sea-alpha-${PAAS_ENV_SHORTNAME}-db-developerkey"

# - Get the key (and print to the console)
cf service-key "sea-alpha-${PAAS_ENV_SHORTNAME}-db" "sea-alpha-${PAAS_ENV_SHORTNAME}-db-developerkey"


#-----------
# Create App
# - Create the App
cf create-app "sea-alpha-${PAAS_ENV_SHORTNAME}"

# - Scale to the right size
cf scale "sea-alpha-${PAAS_ENV_SHORTNAME}" -i ${APP_INSTANCES} -k ${APP_DISK} -m ${APP_MEMORY} -f
echo "This will say FAILED, but it has probably worked (it failed to START the app because there isn't currently an app deployed)"

# - Bind app to file storage
cf bind-service "sea-alpha-${PAAS_ENV_SHORTNAME}" "sea-alpha-${PAAS_ENV_SHORTNAME}-filestorage"

# - Bind app to database
cf bind-service "sea-alpha-${PAAS_ENV_SHORTNAME}" "sea-alpha-${PAAS_ENV_SHORTNAME}-db"

# - Add health check
cf set-health-check "sea-alpha-${PAAS_ENV_SHORTNAME}" http --endpoint "//health-check"



#-------------------
# Logit.io log drain
# - Create the log drain service (built by logit.io)
cf create-user-provided-service logit-ssl-drain -l syslog-tls://0f5c243f-b55b-478f-be38-a7bb80036274-ls.logit.io:18298

# - Bind app to logit.io drain
cf bind-service "sea-alpha-${PAAS_ENV_SHORTNAME}" logit-ssl-drain



# Wait for user input - just to make sure the window doens't close without them noticing
echo ""
echo ""
read  -n 1 -p "Press Enter to finish:" unused
