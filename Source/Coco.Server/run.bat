start dotnet build --project ./Coco.IdentityDAL/Coco.IdentityDAL.csproj
start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
start dotnet build --project ./Coco.Entities/Coco.Entities.csproj
start dotnet build --project ./Coco.DAL/Coco.DAL.csproj
start dotnet build --project ./Coco.Contract/Coco.Contract.csproj
start dotnet build --project ./Coco.Common/Coco.Common.csproj
start dotnet build --project ./Coco.Business/Coco.Business.csproj
start dotnet build --project ./Coco.Api.Framework/Coco.Api.Framework.csproj

start dotnet build --project ./Api.Gateway/Api.Gateway.csproj
start dotnet build --project ./Api.Identity/Api.Identity.csproj
start dotnet build --project ./Api.Content/Api.Content.csproj
start dotnet build --project ./Api.Public/Api.Public.csproj
start dotnet build --project ./Api.Resources/Api.Resources.csproj

start /d "." dotnet run --project ./Api.Gateway/Api.Gateway.csproj
start /d "." dotnet run --project ./Api.Identity/Api.Identity.csproj
start /d "." dotnet run --project ./Api.Content/Api.Content.csproj
start /d "." dotnet run --project ./Api.Public/Api.Public.csproj
start /d "." dotnet run --project ./Api.Resources/Api.Resources.csproj