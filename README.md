# Improve Your Property's Energy Efficiency BETA

## Development

The app communicates with a number of APIs. You will need to obtain and configure credentials for these APIs.

Details TBC

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