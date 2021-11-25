<h1 align="center">Gastromio</h1>
<p align="center">Online Essen bestellen und dabei lokale Gastronomen unterstützen.</p>

<p align="center">
    <a href="https://github.com/marartz/Gastromio/blob/develop/LICENSE" target="_blank">
        <img src="https://img.shields.io/github/license/marartz/Gastromio?style=flat-square" alt="Gastromio licence" />
    </a>
    <a href="https://github.com/marartz/Gastromio/issues" target="_blank">
        <img src="https://img.shields.io/github/issues/marartz/Gastromio?style=flat-square" alt="Gastromio issues" />
    </a>
    <a href="https://www.gastromio.de/" target="blank">
        <img src="https://betteruptime.com/status-badges/v1/monitor/7q6q.svg" />
    </a>
    <a href="https://gitmoji.carloscuesta.me">
        <img src="https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg?style=flat-square" alt="Gitmoji">
    </a>
</p>

## Testen des Systems 🔎

### Voraussetzungen
- Docker Community Edition
- MongoDB

Das System wird automatisiert mit GitHub Actions gebaut. GitHub Actions baut das Paket (in den Artefakten des Builds)
und ein Dockerimage, welches bei [Docker Hub](https://hub.docker.com/repository/docker/marartz/gastromio) veröffentlich wird

Es werden unterschiedliche Versionen (Tags) veröffentlicht, abhängig vom Branch, der automatisch gebaut wurde. Im Folgenden ist der Platzhalter &lt;tag> durch einen Branch-Namen ersetzt werden (bspw. develop). 
Für Branches mit Slash: Das Slash muss durch
einen Unterstrich ersetzt werden (Beispiel: feature/abc => feature_abc)

Zum Herunterladen des Images:
```
docker pull marartz/gastromio:<tag>
```

Zum Starten des Images:
```
docker run -p80:80 -e CONNECTIONSTRINGS__MONGODB=mongodb://host.docker.internal:27017 marartz/gastromio:<tag>
```
    
Wenn die Datenbank zurückgesetzt und Testdaten automatisiert angelegt werden sollen:
```
docker run -p80:80 -e CONNECTIONSTRINGS__MONGODB=mongodb://host.docker.internal:27017 -e SEED=true marartz/gastromio:<tag>
```

## Projekt für die Entwicklung aufsetzen 🔨
1. Vorrausgesetzte Software installieren
    * .NET SDK 3.1
    * [MongoDB](https://www.mongodb.com/try/download/community) (+ [Compass](https://www.mongodb.com/try/download/compass) empfohlen)
    * IDE für C# und Webentwicklung (z.B. [Visual Studio](https://visualstudio.microsoft.com/de/))
    * [NodeJS](https://nodejs.org/)
2. Repository klonen: ```git clone```
3. Abhängigkeiten installieren
    * Backend: ```dotnet restore ./src/Gastromio.App```
    * Frontend: ```npm i``` (Ausführen in ./src/Gastromio.App/ClientApp)
4. Frontend starten: ```npm start```
5. Backend starten: ```dotnet run --project ./src/Gastromio.App```
6. Im Browser ```localhost:5001``` aufrufen
   - **Hinweis**: Port kann varieren

### Installation von MongoDB
[MongoDB](https://www.mongodb.com/try/download/community) kann entweder auf dem Entwicklungs- oder Testrechner installiert werden oder mit Hilfe von Docker:

```
docker pull mongo:latest
docker run -d -p 27017-27019:27017-27019 --name mongodb mongo:latest
```

Ein Admin-User-Interface kann hier heruntergeladen werden: https://docs.mongodb.com/compass/master/install/

## Lizenzierung 📑

Copyright (c) 2021 Marco Artz. All Rights reserved.

Licensed under the **MIT License** (the "License").
You may obtain a copy of the License at https://opensource.org/licenses/MIT.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the conditions mentioned in [LICENSE](./LICENSE).
