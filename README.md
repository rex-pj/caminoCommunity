# Camino Community

An open-source to build a place to connect many people and build communities with special industries and hobbies example farmer, food production, Camino is the appropriate source.
Hope to have your comments to build and improve Camino better.

**Main technologies:**
- ASP.NET 5.0
- Hot Chocolate for GraphQL
- ReactJS
- Apollo Client

**Business structure:**
- Client
  + **client-app** will take over the role for the front-end for the client, it's based on ReactJS and Apollo Client to *interact with Camino.ApiHost*
- Server:
  + **Camino.ApiHost** is an API server based on ASP.NET 5.0 and Hot Chocolate to build a GraphQL API, serve for **client-app**.
  + **Camino.Management** based on ASP.NET 5.0 MVC, it will take the role for some specific person to manage the data as Administrator, Moderator, Approver.
  
**Please note:**
- Both **Camino.ApiHost** and **Camino.Management** is built on the Modular concept, please take a look at Modules directory to see their modules.
  + The modules have the prefix like *Module.Api.* is written for **Camino.ApiHost**
  + The modules have the prefix like *Module.Web.* is written for **Camino.Management**
- On Visual Studio 2019, *to make sure all modules are built and their assemblies are created*, before you run or debug please **build the solution** first to allow all DLLs are created.
