# FoodOrderSystem

## Voraussetzungen für Entwickler
- .NET SDK 3.1
- MongoDB

## Voraussetzungen für Tester
- Docker Community Edition
- MongoDB

## Installation von MongoDB
MongoDB kann entweder auf dem Entwicklungs- oder Testrechner installiert werden (https://www.mongodb.com/try/download/community)
oder mit Hilfe von Docker:

    docker pull mongo:latest
    docker run -d -p 27017-27019:27017-27019 --name mongodb mongo:latest

## Testen des Systems
Das System wird automatisiert mit GitHub Actions gebaut. GitHub Actions baut das Paket (in den Artefakten des Builds)
und ein Dockerimage, welches bei Docker Hub veröffentlich wird: https://hub.docker.com/repository/docker/marartz/food_order_system

Zum Herunterladen des Images:
    
    docker pull marartz/food_order_system:<tag>

Zum Starten des Images:
    
    docker run -p80:80 -e CONNECTIONSTRINGS__MONGODB=mongodb://host.docker.internal:27017 marartz/food_order_system:<tag>
    
Wenn die Datenbank zurückgesetzt und Testdaten automatisiert angelegt werden sollen:
    
    docker run -p80:80 -e CONNECTIONSTRINGS__MONGODB=mongodb://host.docker.internal:27017 -e SEED=true marartz/food_order_system:<tag>

*Wichtig*: <tag> muss durch einen Branch-Namen ersetzt werden (bspw. develop). Für Branches mit Slash: Das Slash muss durch
einen Unterstrich ersetzt werden (Beispiel: feature/abc => feature_abc)
