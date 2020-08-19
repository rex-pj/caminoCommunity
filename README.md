# Camino Social

An open-source to build a place to connect many people and build communities with special industries and hobbies example farmer, food production, Camino is the appropriate source.
Hope to have your comments to build and improve Camino better.

**Main technologies:**
- ASP.NET Core 3.1
- Hot Chocolate for GraphQL
- ReactJS
- Apollo Client

**Business structure:**
- Client
  + **Camino.ApiHost** is an API server based on ASP.NET CORE 3.0 and Hot Chocolate to build a GraphQL API.
  + **clientApp** will take over the role for the front-end for the client, it's based on ReactJS and Apollo Client to *interact with Camino.ApiHost*
- Management Center:
  + **Camino.Management** based on ASP.NET CORE 3.0 MVC, it will take the role for some specific person to manage the data as Administrator, Moderator, Approver.
  
**Please note:**
- Both **Camino.ApiHost** and **Camino.Management** is built on the Modular concept, please take a look at Modules directory to see their modules.
  + The modules have the prefix like *Module.Api.****** is written for **Camino.ApiHost**
  + The modules have the prefix like *Module.Api.****** is written for **Camino.Management**
- On Visual Studio 2019, *to make sure all modules are built and their assemblies are created, before you run or debug please **build the solution** first to allow all DLLs are created.
