# Find energy improvements suitable for your home BETA

## Deployment

The site is deployed using github actions.

### Database Migrations

Migrations will be run automatically on deployment. If a migration needs to be rolled back for any reason there are two options:
1. Create a new inverse migration and deploy that
2. Generate and run a rollback script
   1. Check out the same commit locally
   2. [Install EF Core CLI tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet) if you haven't already
   3. Generate a rollback script using `dotnet ef migrations script 2022010112345678_BadMigration 2022010112345678_LastGoodMigration -o revert.sql` from the `SeaPublicWebsite` directory
   4. Review the script 
   5. Connect to the database using cf conduit
       1. If you haven't used conduit before: `cf install-plugin conduit`
       2. `cf conduit sea-beta-<Environment>-db`
   6. Use pgAdmin or similar with the credentials from cf conduit to run the rollback script

### Code cleanup/formatting/linting

We use the standard Rider code cleanup tool for this project. Before committing, make sure to run the code cleanup on edited files.
See [Rider docs](https://www.jetbrains.com/help/rider/2024.3/Code_Cleanup__Index.html#running) for information on running the formatter. Use the 'DESNZ' profile when running code format.

Historically, we did not always use this formatter, so some files will be non-compliant.
Run the formatter on all files edited in a PR. There may be additional formatting changes made.
Commit these in a separate commit to your other changes.

## Development

### Process

We have three branches linked to environments: dev, staging, and main.
Committing to any of these will trigger a release to the relevant environment.

For normal development:
- Create a branch from dev
- Make changes on the branch
- Raise a PR to dev once the feature is complete
- If the PR is accepted merge the branch into dev and check it works
- When we are ready for UAT merge dev into staging

Doing a release:
- Confirm the current version of staging has passed UAT
- Gain sign-off to deploy
- Merge staging into main
   - To merge to main, the `production release` label must be applied to your pull request

For critical bug fixes on production
- Create a branch from main
- Make changes on the branch
- Raise a PR back to main once the bug is fixed
   - To merge to main, the `production release` label must be applied to your pull request
- If the PR is accepted merge the branch

### Deployments

When code is merged to the dev, staging or main branch, the corresponding CodeDeploy pipeline is started.

You can look at the Deployment history [here](https://eu-west-2.console.aws.amazon.com/codesuite/codedeploy/deployments?region=eu-west-2). It should show you all the successful, failed and in-progress deployments for your current role.

### Infrastructure

Infrastructure is managed with Terraform in the [beis-sea-app](https://github.com/databarracks/beis-sea-app) repository.

The README should have all the information you need. Infrastructure changes are triggered automatically by merges to the main branch.

In the case you need to manually trigger an infrastructure deployment, you will need to contact Data Barracks directly.

### Pre-requisites

- .Net 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Install EF Core CLI tools (https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- Node v14+ (https://nodejs.org/en/)
- If you're using Rider then you will need to install the ".net core user secrets" plugin

In SeaPublicWebsite run `npm install`

### GovUkDesignSystem

We are using the GovUkDesignSystem library from the Cabinet Office: https://github.com/cabinetoffice/govuk-design-system-dotnet

As this library is not currently published to Nuget we have a copy of the library in a nuget package in the /Lib folder of this solution.

If you need to make changes to the GovUkDesignSystem (e.g. to add a new component) then you should:
- Clone the BEIS fork of the repository (currently https://github.com/UKGovernmentBEIS/govuk-design-system-dotnet) and check out the `master` branch.
- Create a branch for you feature
- Develop and commit your changes (don't forget automated tests as applicable)
- Build and package your branch with `dotnet pack -p:PackageVersion=1.0.0-$(git rev-parse --short HEAD) -c Release -o .` in the `GovUkDesignSystem` folder
- Copy the built package to /Lib and delete the old package
- Update the package version in the SEA project
- Test that your changes work on the SEA site
- Create a PR from your branch back to `master`
- Get the PR reviewed and merged
- From time to time create a PR to merge the `master` branch back to the Cabinet Office repository (https://github.com/cabinetoffice/govuk-design-system-dotnet)

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

### Running Locally

- In Rider, select a new build configuration for docker-compose, selecting the docker-compose.yml file. You should then be able to debug by pressing F5.
- NOTE: The postgres instance runs on port 5432, which is the default postgres port. If you are running any other local postgres instance, it is likely they will fight for this port.

### Creating/updating the local database

- You can just run the website project and it will create and update the database on startup
- If you want to manually update the database (e.g. to test a new migration) in the terminal (from the solution directory) run `dotnet ef database update --project .\SeaPublicWebsite`

### Adding Migrations

- In the terminal (from the solution directory) run `dotnet ef migrations add <YOUR_MIGRATION_NAME> --project .\SeaPublicWebsite.Data --startup-project .\SeaPublicWebsite`
- Then update the local database

### Reverting Migrations

You may want to revert a migration on your local database as part of a merge, or just because it's wrong and you need to fix it (only do this for migrations that haven't been merged to main yet)
- Run `dotnet ef database update <MIGRATION_BEFORE_YOURS> --project .\SeaPublicWebsite` to rollback your local database
- Run `dotnet ef migrations remove --project .\SeaPublicWebsite.Data --startup-project .\SeaPublicWebsite` to delete the migration and undo the snapshot changes

#### Merging Migrations

We cannot merge branches both containing different migrations. We have marked the EF Core snapshot file as binary in git. This should mean that git throws up an error if we try to merge branches with different migrations
(because git will try to merge two sets of changes into the snapshot file and it can't merge changes in binary files).
The solution is unfortunately tedious. Given branch 1 with migration A and branch 2 with migration B:
- One branch is merged to main as normal (let's say branch 1)
- On branch 2
- Revert and remove migration B
- Merge main into branch 2
- Recreate migration B (which will now be on top of migation A)
- Merge branch 2 into main

## Environments

This app is deployed to AWS via docker containers hosted on ECS (https://docs.aws.amazon.com/ecs/)

### Pre-requisites

Before you can work with the environments you will need some things:
- AWS Credentials with access to the following roles:
  - SEA-Development
  - SEA-Staging
  - SEA-Production  
- Access to the [beis-sea-app](https://github.com/databarracks/beis-sea-app) repository

### Set up

To set up an environment for the first time:
- Contact Data Barracks as they manage the terraform code

### Environment Variables
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

When working with Dev, Staging or Prod environments, the aforementioned variables are set via the [Parameter Store](https://eu-west-2.console.aws.amazon.com/systems-manager/parameters/?region=eu-west-2&tab=Table)

### Logs

To look at requests made to the server we can check [Logit](https://dashboard.logit.io/) or [AWS CloudWatch](https://eu-west-2.console.aws.amazon.com/cloudwatch/home?region=eu-west-2#home:)

On Logit, you can Launch the Kibana interface and query the logs using [KQL](https://www.elastic.co/guide/en/kibana/7.10/kuery-query.html).
Logit crendentials are stored in Keeper.

On CloudWatch, you can look at Logs, Alarms and Metrics all displayed in the right-hand menu


### Google Analytics

Ask a member of the team to grant you access to the BEIS DCEAS Google Analytics Account.

To debug GA on any of the dev, staging or prod websites you can use the [Google Tag Assistant](https://tagassistant.google.com/)

The Staging and Dev environment share GA credentials
