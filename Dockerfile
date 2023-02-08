FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
ENV TZ=Europe/Berlin
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY ./src/Gastromio.App/bin/Release/net7.0/publish ./
ENTRYPOINT ["dotnet", "Gastromio.App.dll"]
