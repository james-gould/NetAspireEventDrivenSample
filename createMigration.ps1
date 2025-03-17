$guid = New-Guid

echo "executing dotnet build"

dotnet build -v:q

echo "changing directory to DbContext"

cd .\ProjectBase.Data

echo "executing dotnet ef migrations with migration name ${guid}"


# move into the project with our DbContext and create a migration, setting the startup project to an application that uses AddDbContext<TContext>
 dotnet ef migrations add $guid --startup-project ../ProjectBase.Api --no-build

echo "completed build and migrations!"

cd ..