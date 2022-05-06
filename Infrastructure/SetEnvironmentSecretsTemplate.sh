
# Usage
# 1) Copy this file to SetEnvironmentSecrets.sh
# 2) Replace all occurrences of <REAL_VALUE_HERE> with real credential values
# 3) Check that you have LoginToGovPaas.sh set up
# 4) Run SetEnvironmentSecrets.sh

#------
# Login
./LoginToGovPaas.sh

# Name - This is the bit after 'sea-alpha-' - e.g. for sea-alpha-DEV, PAAS_ENV_SHORTNAME would just be 'DEV'
PAAS_ENV_SHORTNAME="<REAL_VALUE_HERE>"

# DfLUHC EPC API Credentials
cf set-env "sea-alpha-${PAAS_ENV_SHORTNAME}" EpbEpcAuthUsername "<REAL_VALUE_HERE>"
cf set-env "sea-alpha-${PAAS_ENV_SHORTNAME}" EpbEpcAuthPassword "<REAL_VALUE_HERE>"

# BRE API Credentials
cf set-env "sea-alpha-${PAAS_ENV_SHORTNAME}" BreUsername "<REAL_VALUE_HERE>"
cf set-env "sea-alpha-${PAAS_ENV_SHORTNAME}" BrePassword "<REAL_VALUE_HERE>"

# GovUK Notify Credentials
cf set-env "sea-alpha-${PAAS_ENV_SHORTNAME}" GovUkNotifyApiKey "<REAL_VALUE_HERE>"
