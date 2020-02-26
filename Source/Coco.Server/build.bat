rem start dotnet build --project ./Coco.IdentityDAL/Coco.IdentityDAL.csproj
rem start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
rem start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
rem start dotnet build --project ./Coco.DAL/Coco.DAL.csproj
rem start dotnet build --project ./Coco.Contract/Coco.Contract.csproj
rem start dotnet build --project ./Coco.Common/Coco.Common.csproj
rem start dotnet build --project ./Coco.Business/Coco.Business.csproj
rem start dotnet build --project ./Coco.Api.Framework/Coco.Api.Framework.csproj

rem start dotnet build --project ./Api.Gateway/Api.Gateway.csproj
rem start dotnet build --project ./Api.Identity/Api.Identity.csproj
rem start dotnet build --project ./Api.Content/Api.Content.csproj
rem start dotnet build --project ./Api.Public/Api.Public.csproj
rem start dotnet build --project ./Api.Resources/Api.Resources.csproj

start dotnet msbuild ./Api.Gateway/Api.Gateway.csproj
start dotnet msbuild ./Api.Identity/Api.Identity.csproj
start dotnet msbuild ./Api.Content/Api.Content.csproj
start dotnet msbuild ./Api.Public/Api.Public.csproj
start dotnet msbuild ./Api.Resources/Api.Resources.csproj