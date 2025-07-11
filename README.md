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

Clone the application down ensure you have a container host running, either [Docker Desktop](https://www.docker.com/products/docker-desktop/) or [Podman](https://podman.io/).

# Launch

Now we're ready! Start debugging with your launch application set as `ProjectBase.AppHost` and the dashboard will open, with all services running against a debugger.
