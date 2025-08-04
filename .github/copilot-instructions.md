# GitHub Copilot Agent Instructions

## Architektur Dokumentation

Die Architektur Dokumentation für trailmarks ist unter https://trailmarks-io.github.io/docs/architecture einsehbar.

## Architektur Entscheidungen

Wenn Entscheidungen hinsichtlich der Architektur notwendig sind, erstelle bitte ein Issue im [trailmarks-io/docs Repository](https://github.com/trailmarks-io/docs) auf GitHub.

## Backend Technologie

Das Backend wird mit C# entwickelt.

## Akzeptanzkriterien für Issue #3

Für die Implementierung der Wandersteine-Übersichtsseite gelten folgende Akzeptanzkriterien:

- Die Daten werden mittels C# ASP.NET Core aus der Datenbank PostgreSQL geladen
- Das Backend bereitet die Daten bei API Endpunkt auf
- Der API Endpunkt unterstützt das OpenAPI Format
- Die Oberfläche wird mit Angular gebaut