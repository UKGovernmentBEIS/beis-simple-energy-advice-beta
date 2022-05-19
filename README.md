# Improve Your Property's Energy Efficiency BETA

## Development

### Pre-requisites

- .Net 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Node v14+ (https://nodejs.org/en/)
- If you're using Rider then you will need to install the ".net core user secrets" plugin

In SeaPublicWebsite run `npm install`

### GovUkDesignSystem

We are using the GovUkDesignSystem library from the Cabinet Office: https://github.com/cabinetoffice/govuk-design-system-dotnet

As this library is not currently published to Nuget we have a copy of the library in a nuget package in the /Lib folder of this solution.

If you need to make changes to the GovUkDesignSystem (e.g. to add a new component) then you should:
- Clone the BEIS fork of the repository (currently https://github.com/DanCorderSoftwire/govuk-design-system-dotnet) and check out the `sea-changes` branch.
- Create a branch for you feature
- Develop and commit your changes (don't forget automated tests as applicable)
- Build and package your branch with `dotnet pack -p:PackageVersion=1.0.0-$(git rev-parse --short HEAD) -c Release -o .` in the `GovUkDesignSystem` folder
- Copy the built package to /Lib and delete the old package
- Update the package version in the IYPEE project
- Test that your changes work on the IYPEE site
- Create a PR from your branch back to `sea-changes`
- Get the PR reviewed and merged
- From time to time create a PR to merge the `sea-changes` branch back to the Cabinet Office repository (https://github.com/cabinetoffice/govuk-design-system-dotnet)

### APIs

The app communicates with a number of APIs. You will need to obtain and configure credentials for these APIs in your user secrets file.

In Rider:
- Right-click on the `SeaPublicWebsite` project
- Select `Tools`
- Select `Open Project User Secrets`

Fill in the opened `secrets.json` file with:

```json
{
    "BasicAuth": {
        "Username": "<REAL_VALUE_HERE>",
        "Password": "<REAL_VALUE_HERE>"
    },
    
    "OpenEpc": {
        "Username": "<REAL_VALUE_HERE>",
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
    }
}
```

### Running Locally

- In Visual Studio / Rider build the solution
- In `SeaPublicWebsite` run `npm run watch`
- In Visual Studio / Rider run the `SeaPublicWebsite` project

## Local Database

### Local Database Setup

- For Windows: Download the installer and PostgreSQL 14 here: https://www.postgresql.org/download/windows/
- Follow default installation steps (no additional software is required from Stack Builder upon completion)
  - You may be prompted for a password for the postgres user and a port (good defaults are "postgres" and "5432", respectively). If you choose your own, you will have to update the connection string in appsettings.json

### Creating/updating the local database

- In the terminal (from the solution directory) run `dotnet ef database update --project .\SeaPublicWebsite`

### Adding Migrations

- In the terminal (from the solution directory) run `dotnet ef migrations add <YOUR_MIGRATION_NAME> --project .\SeaPublicWebsite.Data --startup-project .\SeaPublicWebsite`
- Then update the local database

## Environments

This app is deployed to GOV.UK Platform as a Service (https://docs.cloud.service.gov.uk/)

### Pre-requisites

Before you can work with the environments you will need some things:
- Credentials for GOV.UK PaaS
- Install the CloudFoundry CLI (https://github.com/cloudfoundry/cli/wiki/V7-CLI-Installation-Guide)

### Set up

To set up an environment for the first time:
- CD to `Infrastructure`
- Take a copy of `LoginToGovPaasTemplate.sh`, rename it to `LoginToGovPaas.sh`, and update it with your credentials
- Run `CreateEnvironment.sh`
- Take a copy of `SetEnvironmentSecretsTemplate.sh`, rename it to `SetEnvironmentSecrets.sh`, and fill in the required credentials
- Run `SetEnvironmentSecrets.sh`