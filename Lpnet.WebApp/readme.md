### Run migration

```sh
Open nuget package console
Set as startup project Ats.Web
Package console set default project Ats.build
run command: update-database -context AppIdentityDbContext

Run migration data
Debug => start new instance Ats.Build


/* ***** READ ME

    // Update Database
    // Set project start to Ats.Web (as default)
    // Set default project (in PMC) to Ats.Build
    update-database -context AppIdentityDbContext


    // Seeding Data
    bin/Debug/netcore/dotnet Ats.Build.dll

    Reset DB
    Update-Database -Migration 0 -context AppIdentity


    // Add migration
    //add-migration "file-name" -context AppIdentityDbContext 
    //
    add-migration AddModels-MemberandLoyalty -context AppIdentityDbContext
    //to day
    remove-migration -context AppIdentityDbContext
    //
*/