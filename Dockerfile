FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
ENV TZ=Europe/Berlin
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY ./src/Gastromio.App/bin/Release/netcoreapp3.1/publish ./
ENTRYPOINT ["dotnet", "Gastromio.App.dll"]