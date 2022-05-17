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