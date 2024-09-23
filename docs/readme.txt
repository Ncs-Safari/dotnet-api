This application is a RESTFul API template built using the .NET Framework version 8. The architecture adopts the Clean Architecture model with a layer model according to its function.
1. Domain, this layer contains the core business logic and entities of the application. It is independent of any infrastructure or application details. Keep it clean and focus on representing the business rules and behaviors.
2. Application, this layer orchestrates the flow of data between the Domain layer and the Infrastructure layer. It contains use cases, business logic, and application-specific rules. Use the Repository pattern and Unit of Work to interact with the database.
3. Infrastructure, this contains implementation details like databases, external services, and other tools required for the application to function. Use Dapper as the ORM for database interactions , PostgreSQL as the database, and Docker for containerization.
4. Presentation, the Web API layer exposes endpoints to interact with the application. It handles incoming HTTP requests, maps them to application use cases, and returns appropriate responses. It uses ASP.NET Web API to provide a RESTful interface.
Net Connect Services (ncs.xxxxx@gmail.com)

Foldering Structure:
.
├── config       # contains configuration files was used in frontend and backend application, such as appsettings.json, launchSettings.json, or other environment-specific configurations.
├── documents    # contains documentation related to the project.
│── scripts      # contains scripts for build, deployment, or other automation tasks (include DDL scripts).
└── src          # contains the source code for your application.
    └── backend         # source code backend application with .Net Framework and C# language
        ├── Ncs.sln        # .net solution name
        └── Common         # project is a shared library that contains common code and utilities that are used across multiple services in your solution. This project promotes code reuse and helps maintain consistency across different services.
            ├── Contants       # common constants used in applications such as data status and others.
            ├── Dtos           # common DTO for transferring data between different application layers 
            ├── Entities       # a general class/object can be a database table or a specific class that is used generally in an application.
            ├── Exceptios      # for handling errors or other exceptional conditions that occur during the execution of a program
            ├── Extensions     # methods provide a way to add new methods to existing types (classes, structs, interfaces) without modifying their source code or creating a new derived type
            ├── Interfaces     # defines a contract that specifies a set of members (methods, properties, events, indexers) that a class or struct must implement.
            ├── Utils          # common utilities used in applications can be formulas, conversion functions or others.
            └── ValueObjects   # 
        └── UserManagement # the core module for handling user-related functionalities
            └── Application    # 
                ├── Commands       # 
                ├── Configurations # 
                ├── Dtos           # Table class mapping from database used ORM
                ├── Interfaces     # 
                ├── Queries        # 
                └── Resources      # 
            └── Domain         # 
                ├── Configurations # 
                ├── Contants       # 
                └── Entities       # 
            └── Infrastructure # 
                └── Persistence    # 
                    └── Configurations # 
                └── Services       # 
                    └── Configurations # 
            └── Presentation   # 
                └── Adm.Api       # 
                    ├── Configurations       # 
                    └── Controllers       # 
        └── OtherModule    # the core module for handling another functionalities
    └── frontend        # source code frontend application with NextJs framework and TypeScript language

Depedencies:
1. Use in Common project
    Dapper Version=2.1.35 or latest
    Dapper.Contrib Version=2.0.78 or latest
    Dapper.SimpleCRUD Version=2.3.0 or latest
    log4net Version=2.0.17 or latest
    MailKit Version=4.7.1.1 or latest
    Microsoft.AspNetCore.Http Version=2.2.2 or latest
    Microsoft.Extensions.Localization Version=8.0.7 or latest
    MimeKit Version=4.7.1 or latest
    Newtonsoft.Json Version=13.0.3 or latest
    Npgsql Version=8.0.3 or latest
2. Use in Application project
    AutoMapper.Extensions.Microsoft.DependencyInjection Version=12.0.1 or latest
    FluentValidation Version=11.9.2 or latest
    FluentValidation.AspNetCore Version=11.3.0 or latest
    MediatR Version=11.1.0
    MediatR.Extensions.Microsoft.DependencyInjection Version=11.1.0
    Swashbuckle.AspNetCore.Annotations Version=6.6.2 or latest
3. Use in Services project
    Microsoft.AspNetCore.Authentication.JwtBearer Version=8.0.7 or latest

