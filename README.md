# id.co.danliris-garment-web

DanLiris Application is a enterprise project that aims to manage the business processes of a textile factory, PT. DanLiris.This application is a microservices application consisting of services based on .NET Core and Aurelia Js which part of javascript front-end Framework. This application show how to implement microservice architecture principles. id.co.danliris-garment-web repository is part of service that will serve  management garment business activity.

## Prerequisites
* Windows, Mac or Linux
* [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/vs/whatsnew/)
* [IIS Web Server](https://www.iis.net/) 
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [.NET Core SDK](https://www.microsoft.com/net/download/core#/current) (v2.0.9,  SDK 2.1.202, ASP.NET Core Runtime 2.0.9 )


## Getting Started

- Fork the repository and then clone the repository using command  `git clone https://github/YOUR-USERNAME/id.co.danliris-garment-web.git`  checkout the `dev` branch.

### Command Line

- Install the latest version of the .NET Core SDK from this page <https://www.microsoft.com/net/download/core>
- Next, navigate to root project or wherever your folder is on the command line in administrator mode.
- Create empty database.
- Setting connection to database using Connection Strings in appsettings.json. Your appsettings.json look like this:
```
{
  "Logging": {
    "IncludeScopes": false,
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=YourDbServer;Database=your_parent_database;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
  "ClientId": "your ClientId",
  "Secret": "Your Secret",
  "ASPNETCORE_ENVIRONMENT": "Development"
}
```
and  Your appsettings.Developtment.json look like this :
```
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

- Make sure port application has no conflict, setting port application in launchSettings.json. on package DanLiris.Admin.Web

```
id.co.danliris-garment-web
 ┣ src
 ┃ ┣ System
 ┃ ┃ ┣ DanLiris.Admin.Web
 ┃ ┃ ┃ ┣ Properties 
 ┃ ┃ ┃ ┃ ┣ appsettings.json
```

file launchSettings.json look like this :
```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:57914/",
      "sslPort": 0
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "WebApplication": {
      "commandName": "Project",
      "applicationUrl": "http://localhost:5005",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

- Call `dotnet run`.
- Then open the `http://localhost:57914/swagger/index.html` URL in your browser.


### Visual Studio
- Download Visual Studio 2019 (any edition) from https://www.visualstudio.com/downloads/ .
- Open `DanLiris.Garment.sln` and wait for Visual Studio to restore all Nuget packages.
- Create empty database.
- Setting connection to database using Connection Strings in appsettings.json. Your appsettings.json look like this:

```
{
  "Logging": {
    "IncludeScopes": false,
    "Debug": {
      "LogLevel": {
        "Default": "Warning"
      }
    },
    "Console": {
      "LogLevel": {
        "Default": "Warning"
      }
    }
  },

  "ConnectionStrings": {
    "DefaultConnection": "Server=YourDbServer;Database=your_parent_database;Trusted_Connection=True;MultipleActiveResultSets=true",
  },
  "ClientId": "your ClientId",
  "Secret": "Your Secret",
  "ASPNETCORE_ENVIRONMENT": "Development"
}
```
and  Your appsettings.Developtment.json look like this :
```
{
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

- Make sure port application has no conflict, setting port application in launchSettings.json. on package DanLiris.Admin.Web

```
id.co.danliris-garment-web
 ┣ src
 ┃ ┣ System
 ┃ ┃ ┣ DanLiris.Admin.Web
 ┃ ┃ ┃ ┣ Properties 
 ┃ ┃ ┃ ┃ ┣ appsettings.json
```

file launchSettings.json look like this :
```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:52409/",
      "sslPort": 0
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Com.Danliris.Service.Finance.Accounting.WebApi": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "http://localhost:5000"
    }
  }
}
```

- Ensure `DanLiris.Admin.Web` is the startup project and run it and the browser will launched in new tab http://localhost:57914/swagger/index.html

### Run Unit Tests in Visual Studio 
1. You can run all test suite, specific test suite or specific test case on test explorer.
2. Choose Tab Menu **Test** to select differnt menu test.
3. Select **Run All Test** or press (Ctrl + R, A ) to run all test suite.
4. Select **Test Explorer** or press (Ctrl + E, T ) to determine  test suite to run specifically.
5. Select **Analyze Code Coverage For All Test** to generate code coverage. 


## Knows More Details
### Directory Tree and description

The modules in this application are built with the [ExtCore](https://github.com/ExtCore/ExtCore)  framework.[ExtCore](https://github.com/ExtCore/ExtCore) is free, open source and cross-platform framework for creating modular and extendable web applications based on ASP.NET Core. ExtCore allows to build a web applications from the different independent reusable modules or extensions. Each of these modules or extensions may consist of one or more ASP.NET Core projects and each of these projects may include everything as want as any other ASP.NET Core project.

ExtCore consists of two general packages and four optional basic extensions.

#### General Packages
ExtCore general packages are:
* ExtCore.Infrastructure
* ExtCore.WebApplication

**ExtCore.Infrastructure**

This package describes such basic shared things as IExtension interface and its abstract implementation – ExtensionBase class. Also it contains ExtensionManager class – the central element in the ExtCore types discovering mechanism. Most of the modules or extensions need this package as dependency.

**ExtCore.WebApplication**

This package describes basic web application behavior with Startup abstract class. This behavior includes modules and extensions assemblies discovering, ExtensionManager initialization etc. Any ExtCore web application must inherit its Startup class from ExtCore.WebApplication.Startup class in order to work properly. Also this package contains IAssemblyProvider interface and its implementation – AssemblyProvider class which is used to discover assemblies and might be replaced with the custom one.

**Basic Extensions**

ExtCore basic extensions are:
* ExtCore.Data
* ExtCore.Data.EntityFramework
* ExtCore.Mvc
* ExtCore.Events.

**ExtCore.Data**

By default, ExtCore doesn’t know anything about data and storage, but you can use ExtCore.Data extension to have unified approach to working with data and single storage context among all the extensions. Storage might be represented by a database, a web API, a file structure or anything else.

**ExtCore.Data.EntityFramework**

Currently it supports MySQL, PostgreSql, SQLite, and SQL Server, but it is very easy to add another storage support.

**ExtCore.Mvc**

By default, ExtCore web applications are not MVC ones. MVC support is provided for them by ExtCore.Mvc extension. This extension initializes MVC, makes it possible to use controllers, view components, views (added as resources and/or precompiled), static content (added as resources) from other extensions etc.

**ExtCore.Events**

It can be used by the extension to notify the code in this or any other extension about some events.


### Root directory and description

```
id.co.danliris-garment-web
 ┣ Manufactures.Tests
 ┣ src
 ┣ .gitattributes
 ┣ .gitignore
 ┣ DanLiris.Garment.sln
 ┣ package-lock.json
 ┗ README.md
```

**1. src**

This folder is source of application. This folder contains 3 extension those are Barebones, manufactures and system/infrastucture. Barebones extension contains libaries package that used by application, manufactures extension contains package those are user defined classes. infrastructure extension contains package those are basic shared things as IExtension interface and its abstract implementation – ExtensionBase class. Package Webapplication/DanLiris.Admin.Web  is basic web application behavior with Startup class.

Package tree:

```
id.co.danliris-garment-web
 ┣ src
 ┃ ┣ Barebone Extension
 ┃ ┃ ┣ Barebone
 ┃ ┃ ┣ Barebone.Tests
 ┃ ┣ Manufactures Extension
 ┃ ┃ ┣ Manufactures
 ┃ ┃ ┣ Manufactures.Application
 ┃ ┃ ┣ Manufactures.Data.EntityFrameworkCore
 ┃ ┃ ┣ Manufactures.Domain
 ┃ ┃ ┣ Manufactures.Domain.Events
 ┃ ┃ ┣ Manufactures.Tests
 ┃ ┣ System
 ┃ ┃ ┣ DanLiris.Admin.Web
 ┃ ┃ ┣ Infrastructure
 ┃ ┃ ┣ Infrastructure.Data.EntityFrameworkCore
 ┃ ┃ ┣ Infrastructure.Domain
 ┃ ┃ ┣ Infrastructure.Domain.Events
 ┃ ┃ ┣ Infrastructure.External.DanLirisClient.Microservice

 ```
 **Manufactures**

This package contains Controller. The controller has responsibility to processing data and  HTTP requests and then send it to a web page. All responses from the HTTP requests API are formatted as JSON (JavaScript Object Notation) objects containing information related to the request, and any status.

**Manufactures.Domain**

This package contains models. The Model is a collection of objects that Representation of data structure which hold the application data and it may contain the associated business logic.on of data structure which hold the application data and it may contain the associated business logic.This Package also contains data validation classes to  validate data. Mechanism to validate data using Fluent Validation library.


**Manufactures.Tests**

This folder is collection of classes to run code testing. The automation type testing used in this app is  a unit testing with using moq and xunit libraries.

DataUtils:

- Colecction class to seed data as data input in unit test 

**File Startup.cs**

This file contains Startup class. The Startup class configures services and the app's request pipeline.Optionally includes a ConfigureServices method to configure the app's services. A service is a reusable component that provides app functionality. Services are registered in ConfigureServices and consumed across the app via dependency injection (DI) or ApplicationServices.This class also Includes a Configure method to create the app's request processing pipeline.

**AppStorageContext.cs**

This file contain context class that derives from DbContext in entity framework. DbContext is an important class in Entity Framework API. It is a bridge between domain or entity classes and the database. DbContext and context class  is the primary class that is responsible for interacting with the database.

**File .codecov.yml**

This file is used to configure code coverage in unit tests.

**File .travis.yml**

Travis CI (continuous integration) is configured by adding a file named .travis.yml. This file in a YAML format text file, located in root directory of the repository. This file specifies the programming language used, the desired building and testing environment (including dependencies which must be installed before the software can be built and tested), and various other parameters.

**DanLiris.Garment.sln**

File .sln is extention for *solution* aka file solution for .Net Core, this file is used to manage all project by code editor.

 ### Validation
Data validation using [Fluent Validation](https://github.com/FluentValidation/FluentValidation) 