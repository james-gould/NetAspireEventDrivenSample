using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddAzureProvisioning();

var sql = builder.AddSqlServer("sql").WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("testingDb");

var storage = builder.AddAzureStorage("projectStorage").RunAsEmulator();
var queue = storage.AddQueues("queue");
var blob = storage.AddBlobs("blob");

var bus = builder.AddAzureServiceBus("projectBus").RunAsEmulator(o => o.WithLifetime(ContainerLifetime.Session));

bus.AddServiceBusQueue("consumer");
bus.AddServiceBusQueue("publisher");

builder.AddProject<ProjectBase_MigrationsService>("migration-service")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<ProjectBase_Api>("weather-api")
    .WithReference(bus)
    .WithReference(db)
    .WaitFor(db)
    .WaitFor(bus);

builder.AddAzureFunctionsProject<ProjectBase_QueueConsumer>("consumer-function-app")
    .WithHostStorage(storage)
    .WithReference(queue)
    .WithReference(blob)
    .WithReference(db)
    .WithReference(bus)
    .WaitFor(db)
    .WaitFor(bus);

builder.Build().Run();