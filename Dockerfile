FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY ./src/FoodOrderSystem.App/bin/Release/netcoreapp3.1/publish ./
ENTRYPOINT ["dotnet", "FoodOrderSystem.App.dll"]