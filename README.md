# Introduction

This is a fully fledged sample application hooking up a .NET 8 WebAPI to an Azure Function App via a Service Bus, all hosted via .NET Aspire.

Included:

- SQL Server
  - Database
- Azure Service Bus (emulated)
- Web API
- Azure Function App (Queue Trigger)

# Why?

Building distributed applications is hard, and often annoying. Needing to check several sources of information to validate a request has completed an E2E journey is tedious at best, .NET Aspire has solved this.

# Getting Started

Clone the application down and ensure you have `User Secrets` enabled, then add the following secrets to `ProjectBase.QueueConsumer` and `ProjectBase.Api`:

```
{
  "ConnectionStrings": {
    "projectVault": "",
    "testingDb": ""
  }
}
```

We also need to configure the `ProjectBase.AppHost` project to verify our Azure subscription, although it will **not** be used.

Add the following to the `AppHost` User Secrets:

```
{
  "Azure": {
    "SubscriptionId": "<Your subscription id>",
    "AllowResourceGroupCreation": true,
    "ResourceGroup": "<Valid resource group name>",
    "Location": "<Valid Azure location>"
  }
}
```

You will also need to have a container host running, either [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman](https://podman.io/).

Run `createMigrations.ps1` to create an initial snapshot of the `ProjectBase.Data.DbContext` - this is required on first launch!

# Launch

Now we're ready! Start debugging with your launch application set as `ProjectBase.AppHost` and the dashboard will open, with all services running against a debugger.
