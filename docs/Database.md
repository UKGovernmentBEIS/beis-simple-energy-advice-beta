# Database

## Setup

To set up your local database please see Local Database Setup in the [README](../README.md).

## Creating/updating the local database

- You can just run the website project and it will create and update the database on startup
- If you want to manually update the database (e.g. to test a new migration) in the terminal (from the solution directory) run `dotnet ef database update --project .\SeaPublicWebsite`

## Connecting to the database with Rider

This is a convenient way of viewing/editing the database and its data. To do this:
- Open the database tab (usually on the right)
- Select the `+` icon then select 'Connect to database'
- You will need to enter the following properties manually:
  - Data source: `PostgreSQL`
  - Host: `localhost`
  - Port: `5432`
  - User: `postgres`
  - Password: `postgres`
  - Database: `seadev`
- Select 'Connect to Database'.
- You should now be able to see `seadev@localhost`
- Select the `x of 6` button just next to this and ensure `seadev` is selected.
- Within this, ensure `public` is selected, then within this you should be able to see all of the tables.

## Adding Migrations

- Make any necessary changes to the model in `SeaPublicWebsite.Data/SeaDbContext.cs` or a model class in `SeaPublicWebsite.BusinessLogic/Models`.
- In the terminal (from the solution directory) run `dotnet ef migrations add <YOUR_MIGRATION_NAME> --project SeaPublicWebsite.Data --startup-project SeaPublicWebsite`
- The migration should automatically reflect the changes you made to the model. `SeaDbContextModelSnapshot.cs` should also have been automatically updated.
- Then update the local database

## Reverting Migrations

You may want to revert a migration on your local database as part of a merge, or just because it's wrong and you need to fix it (only do this for migrations that haven't been merged to main yet)
- Run `dotnet ef database update <MIGRATION_BEFORE_YOURS> --project .\SeaPublicWebsite` to rollback your local database
- Run `dotnet ef migrations remove --project .\SeaPublicWebsite.Data --startup-project .\SeaPublicWebsite` to delete the migration and undo the snapshot changes

### Merging Migrations

We cannot merge branches both containing different migrations. We have marked the EF Core snapshot file as binary in git. This should mean that git throws up an error if we try to merge branches with different migrations
(because git will try to merge two sets of changes into the snapshot file and it can't merge changes in binary files).
The solution is unfortunately tedious. Given branch 1 with migration A and branch 2 with migration B:
- One branch is merged to main as normal (let's say branch 1)
- On branch 2
- Revert and remove migration B
- Merge main into branch 2
- Recreate migration B (which will now be on top of migration A)
- Merge branch 2 into main