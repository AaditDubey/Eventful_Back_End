# event-backend
I.	BACKEND
FluentValidation.AspNetCore
FluentValidation can be used within ASP.NET Core web applications to validate incoming models. There are several approaches for doing this:
•	Manual validation
•	Automatic validation (using the ASP.NET validation pipeline)
•	Automatic validation (using a filter)
With manual validation, you inject the validator into your controller (or api endpoint), invoke the validator and act upon the result. This is the most straightforward approach and also the easiest to see what’s happening.
With automatic validation, FluentValidation is invoked automatically by ASP.NET earlier in the pipeline which allows models to be validated before a controller action is invoked.

1.	AutoMapper
A convention-based object-object mapper.
AutoMapper uses a fluent configuration API to define an object-object mapping strategy. AutoMapper uses a convention-based matching algorithm to match up source to destination values. AutoMapper is geared towards model projection scenarios to flatten complex object models to DTOs and other simple objects, whose design is better suited for serialization, communication, messaging, or simply an anti-corruption layer between the domain and application layer.

2.	Serilog.AspNetCore
Serilog logging for ASP.NET Core. This package routes ASP.NET Core log messages through Serilog, so you can get information about ASP.NET's internal operations written to the same Serilog sinks as your application events.

With Serilog.AspNetCore installed and configured, you can write log messages directly through Serilog or any ILogger interface injected by ASP.NET. All loggers will use the same underlying implementation, levels, and destinations.

install .Net SDK, preferable 7.0 or later versions. 
install C# through Extension.

use command dotnet add package FluentValidation.AspNetCore to install it as a dependency
use command dotnet add package AutoMapper to install it as a dependency
use command dotnet add package  Serilog.Sinks.Console to install it as a dependency 

use command dotnet build and dotnet run. In case if it ask to navigate to csproj file, it's under Src/TimeNet (cd Src/TimeNet)

run mongodb from docker or install mongdb
after that change the connection url to your connection

Testing on website: 
front-end: 
https://event-frontend.bisfu.com/

back-end:
https://event-backend.bisfu.com/

admin login email: admin@admin.com
    password: 123456
