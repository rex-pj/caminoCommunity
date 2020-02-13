start dotnet build --project ./Coco.IdentityDAL/Coco.IdentityDAL.csproj
start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
start dotnet build --project ./Coco.DAL/Coco.DAL.csproj
start dotnet build --project ./Coco.Contract/Coco.Contract.csproj
start dotnet build --project ./Coco.Common/Coco.Common.csproj
start dotnet build --project ./Coco.Business/Coco.Business.csproj
start dotnet build --project ./Coco.Api.Framework/Coco.Api.Framework.csproj

start dotnet msbuild --project ./Api.Gateway/Api.Gateway.csproj
start dotnet msbuild --project ./Api.Identity/Api.Identity.csproj
start dotnet msbuild --project ./Api.Content/Api.Content.csproj
start dotnet msbuild --project ./Api.Public/Api.Public.csproj
start dotnet msbuild --project ./Api.Resources/Api.Resources.csproj