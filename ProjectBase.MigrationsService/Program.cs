using ProjectBase.Data;
using ProjectBase.MigrationsService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<ProjectDbContext>("testingDb");

var host = builder.Build();
host.Run();
