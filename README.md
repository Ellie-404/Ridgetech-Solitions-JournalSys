# JournalSys

Journalsystem for en helseinstitusjon, laget som skoleprosjekt. Systemet lar sekretærer og leger administrere pasienter, timer, prøver og notater.
Alle pasienter og leger er fiktive.

## Teknologi

C# / ASP.NET Core (.NET 10)
Razor Pages (frontend) og API-controllere (backend) i ett prosjekt
SQLite-database (`journal.db`) via Entity Framework Core

## Kom i gang lokalt

Krever .NET 10 SDK.

```
git clone [https://github.com/Ellie-404/Ridgetech-Solitions-JournalSys]
dotnet run
```

Addressen står i terminalen (`Now listening on`).
Databasen `journal.db` lages automatisk første gang.
Endrer du en modell, må `journal.db` slettes (vi bruker ikke migrations ennå).

### Testbrukere (kun Development)

| Brukernavn | Passord    | Rolle   |
|------------|------------|---------|
| admin      | Admin1234! | Admin   |
| lege       | Lege1234!  | Lege    |
| sekretær   | Sekr1234!  | Sekretær|

Disse lages bare når `ASPNETCORE_ENVIRONMENT=Development`.

## Roller

** Lege ** og **Sekretær**: lik tilgang til pasienter, timer, prøver, notater og historikk.
** Admin ** (lærer): kan i tillegg lage oppgaver og rette besvarelser.

## Funksjoner

Opprette, søke etter og generere tilfeldige pasienter.
Timebestilling med sjekk av ledig lege, fargemerking og redigering
Prøvekatalog og prøvesvar
Legenotater
Automatisk historikk
Prøveform: lærer lager oppgaver, elever leverer og lærer retter

## Deployment

| Krav | Verdi |
|------|-------|
| Runtime | .NET 10 (ASP.NET Core) |
| Database | SQLite, `journal.db` i mappen appen kjører fra. Må være skrivbar og på persistent volum |
| `JOURNAL_ADMIN_PASSORD` | **Påkrevd.** Passord til brukeren `admin` ved første oppstart. Uten den lages ingen brukere |
| `ASPNETCORE_URLS` | Porten appen lytter på, f.eks. `http://+:8080` |
| `ASPNETCORE_ENVIRONMENT` | Skal **ikke** være `Development` på serveren |
| HTTPS | Appen har HTTPS-redirect. [Beskriv reverse proxy/sertifikat] |

Start med tom database. Ingen andre eksterne tjenester trengs.

## Mappestruktur

```
Controllers/   API-endepunkter
Models/        Databasemodeller (blir tabeller)
Data/          JournalDbContext
Services/      HistoryService, PatientGenerator
Pages/         Razor Pages (frontend)
wwwroot/       CSS, JS, bilder
```

## Kjente begrensninger

Ingen migrations: databasen må slettes ved modellendringer
[Utlogging, brukeradministrasjon, frontend-sider som mangles]
Alle innloggede leger og sekretærer ser alle pasienter
