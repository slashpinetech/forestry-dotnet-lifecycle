[![MIT License](https://img.shields.io/github/license/slashpinetech/forestry-dotnet-lifecycle?color=1F3B2B&style=flat-square)](https://github.com/slashpinetech/forestry-dotnet-lifecycle/blob/main/LICENSE)

# Forestry.NET -- Lifecycle

Forestry .NET is a set of open-source libraries for building modern web
applications using ASP.NET Core.

This lifecycle package adds support for registering actions that will execute
during startup, prior to ASP.NET Core serving web requests.

## Usage

```c#
services.AddLifecycleActions();

services.AddScoped<IStartupAction, MyFirstStartupAction>();
services.AddScoped<IStartupAction, MySecondStartupAction>();
services.AddScoped<IStartupAction, OutputRoutesStartupAction>();
```
