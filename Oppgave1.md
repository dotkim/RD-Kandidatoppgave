# Oppgave 1

Laget en løsning i branchen som heter oppg1-POCO, denne ble videre utarbeidet mens jeg jobbet med oppgave 2, som du kan se resultatet av i oppg2-contacts branchen.

## Aktuelle filer

- [Person.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/Types/Person.cs) (Type)
- [Enterprise.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/Types/Enterprise.cs) (Type)
- [PeopleService.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Services/PeopleService.cs) (Service/Endepunkt)
- [People.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/People.cs) (Routes)
- [Database.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/Database.cs) (Spørringer)

## Tankegang

Tankegangen min etter å ha lest opp på ServiceStack og ORMLite var å lage de gitte tabellene om til POCOs. Jeg laget to nye klasser som heter `Person.cs` og `Enterprise.cs`. Laget også en [sql](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/HelpFiles/DatabaseConceptDesign.sql) fil for å visualisere relasjonene og spørringene:

```sql
CREATE TABLE Enterprise(
	Id      INT IDENTITY(1,1) CONSTRAINT PK_Enterprise PRIMARY KEY,
	[Name]  NVARCHAR(100)     CONSTRAINT UQ_Name UNIQUE
);
GO

CREATE TABLE People(
  Id            INT IDENTITY(1,1) CONSTRAINT PK_People PRIMARY KEY,
  [Name]        NVARCHAR(100),
  EnterpriseId  INT
    CONSTRAINT FK_Enterprise_Id FOREIGN KEY
    REFERENCES Enterprise
);
GO
```

I [Person.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/Types/Person.cs) vil du se hvordan referansen mot Enterprise er satt opp:

```csharp
// Fra Person.cs
[Reference]
public Enterprise Enterprise { get; set; }
public int EnterpriseId { get; set; }

// Da kan man lage ny enterprise enten ved
new Enterprise { Name = "ACME Inc" }
// Eller på Person via
new Person { Name = "Willifred Manford", Enterprise = new Enterprise { Name = "ACME Inc" } }
// Hvis den finnes fra før
new Person { Name = "Finn Erik", Enterprise = db.SingleById<Enterprise>(1), EnterpriseId = 1 }
```

Relasjonen ble laget ut fra denne [lenken](https://github.com/ServiceStack/ServiceStack.OrmLite#create-tables-schemas). Jeg har brukt eksemplet på 1:1 relasjonen, uten å koble noen PersonId mot Enterprise'n. Noe som *kan* føre til problemer hvis hvis man skal f.eks. kunne hente ut en Enterprise med alle dens tilknyttede Persons. Men dette er ikke noe jeg har testet og må nok gjøre det før jeg kan si meg sikker.

## Resultat

Det er laget en ny Service for å hente en person. Den som lå som eksempel er endret til å hente alle brukerne. Disse kan testes ved å bruke den nye "people" ruten.

- http://localhost:61019/people
  - Denne henter alle personene med relaterte tabeller.
- http://localhost:61019/people/1
  - Denne henter bare personen med id 1 og dens rader fra relaterte tabeller.

Her er en person i et JSON skjema:

```json
{
  "id": 1,
  "enterprise": {
    "id": 1,
    "name": "ACME Inc"
  },
  "enterpriseId": 1,
  "name": "Willifred Manford"
}
```