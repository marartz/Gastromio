<h1 align="center">Gastromio</h1>
<p align="center">Online Essen bestellen und dabei lokale Gastronomen unterstützen.</p>

<p align="center">
    <a href="https://github.com/marartz/Gastromio/LICENSE" target="_blank">
        <img src="https://img.shields.io/github/license/marartz/Gastromio?style=flat-square&logo=github&logoColor=white" alt="Gastromio licence" />
    </a>
    <a href="https://github.com/marartz/Gastromio/issues" target="_blank">
        <img src="https://img.shields.io/github/issues-raw/marartz/Gastromio?style=flat-square&logo=github&logoColor=white" alt="Gastromio issues" />
    </a>
    <a href="https://www.gastromio.de/" target="blank">
        <img src="https://betteruptime.com/status-badges/v1/monitor/7q6q.svg" />
    </a>
</p>

<br />


<div align="center">

**[HINTERGRUND](https://github.com/marartz/Gastromio/README.md#-projekthintergrund) • 
[MITWIRKEN](https://github.com/marartz/Gastromio/README.md#%EF%B8%8F-mitwirken) • 
[UNTERSTÜTZEN](https://github.com/marartz/Gastromio/README.md#-unterst%C3%BCtze-uns) • 
[LIZENZ](https://github.com/marartz/Gastromio/README.md#%EF%B8%8F-lizenzierung)**

</div>

---

<br />

# 🧐 Projekthintergrund

> Mit Gastromio möchten wir das Angebot der Gastronomie erweitern und sie in die Lage versetzen, jederzeit angepasst an die geltenden Bestimmungen ihr Angebot anzubieten. Im- oder außerhalb vom Lokal, zur Abholung oder Lieferung nach Hause.
> 
> Gastromio ist eine ehrenamtliche Initiative der Corona Hilfe Bocholt, die das Ziel hat, die bunte Vielfalt an Gastronomie zu erhalten und daher völlig kostenfrei für jeden, der darüber bestellen oder sein Angebot einstellen möchte.

**Mehr findest du auf der Homepage der [Corona Hilfe Bocholt](https://coronahilfe-bocholt.de/de/).**

# ✍️ Mitwirken
### Testen des Systems

Voraussetzungen
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

### Projekt für die Entwicklung aufsetzen
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

# 🌟 Unterstütze uns!

Wenn du dich bedanken und/oder die aktive Weiterentwicklung von Gastromio unterstützen möchtest:

- Gib dem GitHub Projekt einen Stern!
- Das Projekt in den sozialen Medien verbreiten!
  - Tagge [@gastromio.de](https://www.instagram.com/gastromio.de/) und/oder `#Gastromio`
- Hinterlasse uns eine Bewertung [auf Google](https://g.page/r/CR0IONVwaT6kEAI/review)!

# ⚠️ Lizenzierung

Gastromio ist freie Open-Source-Software, die unter der GNU General Public License v3.0 lizenziert ist. Alle Designs wurden von [Gastromio](https://github.com/Gastromio) erstellt und unter der Creative Commons license (CC BY-SA 4.0 International) vertrieben.
