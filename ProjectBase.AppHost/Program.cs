using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var db = builder
    .AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("testingDb");

var storage = builder.AddAzureStorage("projectStorage").RunAsEmulator();
var queue = storage.AddQueues("queue");
var blob = storage.AddBlobs("blob");

var bus = builder
    .AddAzureServiceBus("projectBus")
    .RunAsEmulator(o => o.WithLifetime(ContainerLifetime.Session));

bus.AddServiceBusQueue("consumer");
bus.AddServiceBusQueue("publisher");

var worker = builder
                .AddProject<ProjectBase_MigrationsService>("migration-service")
                .WithReference(db)
                .WaitFor(db);

builder.AddProject<ProjectBase_Api>("weather-api")
    .WithReference(bus)
    .WithReference(db)
    .WaitFor(bus)
    .WaitForCompletion(worker);

builder.AddAzureFunctionsProject<ProjectBase_QueueConsumer>("consumer-function-app")
    .WithHostStorage(storage)
    .WithReference(queue)
    .WithReference(blob)
    .WithReference(db)
    .WithReference(bus)
    .WaitFor(bus)
    .WaitForCompletion(worker);

builder.Build().Run();