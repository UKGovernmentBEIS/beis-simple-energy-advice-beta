# Find energy improvements suitable for your home BETA

## Local setup

### Pre-requisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for running app locally.
- .Net 8 (https://dotnet.microsoft.com/en-us/download/dotnet/8.0) for editing the code & running tests.
- Install EF Core CLI tools (https://docs.microsoft.com/en-us/ef/core/cli/dotnet) for managing database migrations.
- Node v14+ (https://nodejs.org/en/) for running frontend tasks, such as updating GDS assets.
- If you're using Rider then you will need to install the ".net core user secrets" plugin.

### APIs

The app communicates with a number of APIs. You will need to obtain and configure credentials for these APIs in your user secrets file.

In Rider:
- Right-click on the `SeaPublicWebsite` project
- Select `Tools`
- Select `Open Project User Secrets`

Fill in the opened `secrets.json` file with:

```json
{
    "Auth": {
        "Password": "<REAL_VALUE_HERE>"
    },

    "EpbEpc": {
        "Username": "<REAL_VALUE_HERE>",
        "Password": "<REAL_VALUE_HERE>"
    },
  
    "Bre": {
        "Username": "<REAL_VALUE_HERE>",
        "Password": "<REAL_VALUE_HERE>"
    },

    "GovUkNotify": {
        "ApiKey": "<REAL_VALUE_HERE>"
    },
   
    "GoogleAnalytics": {
        "ApiSecret": "REAL_VALUE_HERE",
        "MeasurementId": "REAL_VALUE_HERE"
    }
}
```

### Local database management

The database will be automatically created when running the project in docker.

For managing the database, see [here](Documentation/database.md).

### Running the service locally

- In Rider, select a new build configuration for docker-compose, selecting the docker-compose.yml file. You should then be able to debug by pressing F5.
- NOTE: The postgres instance runs on port 5432, which is the default postgres port. If you are running any other local postgres instance, it is likely they will fight for this port.

### Running tests

- In the project explorer on the left, right click on `SeaPublicWebsite.UnitTests` and select `Run Unit Tests`.

### Making frontend changes

For instructions on making changes to the frontend, see [here](docs/MakingFrontendChanges.md).

### Auto-Formatter

We use the standard Rider code cleanup tool for this project. Before committing, make sure to run the code cleanup on edited files.
See [Rider docs](https://www.jetbrains.com/help/rider/2024.3/Code_Cleanup__Index.html#running) for information on running the formatter. Use the 'DESNZ' profile when running code format.

Historically, we did not always use this formatter, so some files will be non-compliant.
Run the formatter on all files edited in a PR. There may be additional formatting changes made.
Commit these in a separate commit to your other changes.

## Deployment

The site is deployed using AWS CodePipeline.

### Deployed environments

We follow a process similar to git-flow, with 3 branches corresponding to each of the environments:
- `develop` - [Dev](https://dev.find-energy-improvements-suitable-for-your-home.service.gov.uk/energy-efficiency/new-or-returning-user)
- `staging` - [UAT](https://uat.find-energy-improvements-suitable-for-your-home.service.gov.uk/energy-efficiency/new-or-returning-user)
- `main` - [Production](https://find-energy-improvements-suitable-for-your-home.service.gov.uk/energy-efficiency/new-or-returning-user)

### Trivy

On each push to develop, we run a Trivy scan on the Docker image to check for vulnerabilities.
If the scan fails, we should look into the new vulnerability and either:
- Fix it
- Add to .trivyignore if the issue is a false positive.

### Database Migrations

Migrations will be run automatically on deployment. If a migration needs to be rolled back for any reason there are two options:
1. Create a new inverse migration and deploy that
2. Generate and run a rollback script
   1. Check out the same commit locally
   2. [Install EF Core CLI tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) if you haven't already
   3. Generate a rollback script using `dotnet ef migrations script 2022010112345678_BadMigration 2022010112345678_LastGoodMigration -o revert.sql` from the `SeaPublicWebsite` directory
   4. Review the script
   5. Connect to the database and run the script

### Deployments

When code is merged to the dev, staging or main branch, the corresponding CodeDeploy pipeline is started.

You can look at the Deployment history [here](https://eu-west-2.console.aws.amazon.com/codesuite/codedeploy/deployments?region=eu-west-2). It should show you all the successful, failed and in-progress deployments for your current role.

## Environments

This app is deployed to ICS AWS platform

### Configuration

Non-secret configuration is stored in the corresponding `appsettings.<environment>.json` file:
- appsettings.DEV.json
- appsettings.UAT.json
- appsettings.Production.json

The following is a list of environment variables required by the application. Other variables are needed, but these are set by `appsettings.json` and do not need to be environment variables.

- ASPNETCORE_ENVIRONMENT
  - Set automatically by .NET
- DOTNET_ENVIRONMENT
  - Set automatically by .NET
- Auth__Password
- Bre__Password
- Bre__Username
- EpbEpc__Password
- EpbEpc__Username
- GoogleAnalytics__ApiSecret
- GovUkNotify__ApiKey
- PostgreSQLConnectionseards

When working with Dev, Staging or Prod environments, the aforementioned variables are set via the [Parameter Store](https://eu-west-2.console.aws.amazon.com/systems-manager/parameters/?region=eu-west-2&tab=Table), or directly through the Task Definition.

## Common issues

### The styling doesn't look correct locally

This is likely because the CSS hasn't been built. To fix this:

- Open the terminal in the root folder of the project
- Run `git pull` to ensure you have the latest code
- Navigate to the `SeaPublicWebsite` directory
- Run `npm install` to install any new dependencies
- Run `npm run build` to build the CSS
