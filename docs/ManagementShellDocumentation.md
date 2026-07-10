# Management Shell Documentation

`SeaPublicWebsite.ManagementShell` is a CLI tool for running management scripts.

Scripts can be run either via a Rider run configuration or directly in a Docker container.

### Rider

- Select the drop-down icon to the left of the play icon in the top right.
- Select `Edit configurations`
- Select `+` in the top-left -> .Net Project
- Update Project to `SeaPublicWebsite.ManagementShell`
- Add the name of the script you want (see below) to run followed by any relevant arguments in program arguments.
  - Make sure you've also added the following environment variable: `ConnectionStrings__PostgreSQLConnection: UserId=postgres;Password=postgres;Server=localhost;Port=5432;Database=seadev;Include Error Detail=true;Pooling=true`
- Select `OK` in the bottom right.
- You can now select and run this script.

### Docker

- Find the container ID by running `docker ps` or via Docker Desktop.
- Open a shell in the container: `docker exec -it <CONTAINER_ID> /bin/bash`
- Navigate to the CLI directory: `cd cli`
- Run the desired script: `./SeaPublicWebsite.ManagementShell <COMMAND>`

## List of scripts

- `SetEmergencyMaintenanceState [Enabled/Disabled]` - Enable or disable emergency maintenance mode. When enabled, all requests to the portal are blocked with a 503 response. Only use this as part of the disaster response plan to block all public access to the site.
