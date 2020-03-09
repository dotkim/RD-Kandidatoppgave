# Oppgave 2

Her laget jeg en løsning hvor kontaktene er relatert til en person i people tabellen. Tar også med et eksempel på hvordan jeg ville gjort det annerledes med tanke på at dere skrev: "Kontaktene trenger ikke være gjensidige."

## Aktuelle filer

- [Person.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/Types/Person.cs) (Type)
- [Contact.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/Types/Contact.cs) (Type)
- [PeopleContact.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Models/PeopleContact.cs) (Routes)
- [PeopleContactService.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/People.Services/PeopleContactService.cs) (Service/Endepunkt)
- [Database.cs](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/WebApplication1/Database.cs#L95) (Spørring)

## Tankegang

Når jeg tenker kontakter tenker jeg to måter å gjøre det på i denne løsningen. En hvor kontaktene er relatert til en person i people tabellen (denne laget jeg), og en hvor en kontakt er "selvstendig" i sin tabell.

```sql
-- Relatert til people tabellen, denne er litt annerledes i en POCO!
-- Her er tanken at ContactPerson ligger i people tabellen.
CREATE TABLE Contacts(
  PersonId        INT NOT NULL,
  ContactPersonId INT NOT NULL,
    CONSTRAINT  CK_Contacts_PersonId_ContactPersonId
    PRIMARY KEY (PersonId, ContactPersonId)
);
GO

-- Kontaktpersonen er ikke relatert til people tabellen, og er sin egen oppføring.
CREATE TABLE Contacts(
  Id        INT IDENTITY(1,1) CONSTRAINT FK_Contacts PRIMARY KEY,
  PersonId  INT
    CONSTRAINT FK_People_PersonId FOREIGN KEY
    REFERENCES People(Id),
  [Name]    NVARCHAR(100),
  Mail      NVARCHAR(255)
);
GO
```

Når man ser på disse to i kode:

```csharp
// Likt som SQL over, denne er relatert tilbake til people.
[UniqueConstraint(nameof(PersonId), nameof(ContactPersonId))]
public class Contact
{
  [AutoIncrement]
  public int Id { get; set; }

  [References(typeof(Person))]
  public int PersonId { get; set; }

  [References(typeof(Person))]
  public Person ContactPerson { get; set; }
  public int ContactPersonId { get; set; }
}
// Kontakter kan da lages på denne måten:
new Contact { PersonId = 1, ContactPerson = db.SingleById<Person>(2), ContactPersonId = 2 }

// Ikke relatert tilbake, PersonId blir en foreign key.
[UniqueConstraint(nameof(PersonId), nameof(ContactPersonId))]
public class Contact
{
  [AutoIncrement]
  public int Id { get; set; }
  public int PersonId { get; set; }

  [StringLength(StringLengthAttribute.MaxText)]
  public string Name { get; set; }
  [StringLength(StringLengthAttribute.MaxText)]
  public string Mail { get; set; }
}
// Kontaker kan lages på denne måten:
new Person
{
  Name = "Willifred Manford",
  Enterprise = new Enterprise { Name = "ACME Inc" },
  Contacts = new List<Contact>
  {
    new Contact { Name = "Kai Kaia", Mail = "Kai@kaia.no" }
  }
}
// Eller
new Contact { PersonId =1, Name = "Kai Kaia", Mail = "Kai@kaia.no" }
```

Slik vil det se ut på Person.cs, denne er basert på [customer](https://github.com/ServiceStack/ServiceStack.OrmLite#reference-support-poco-style) og order tabellen fra den lenken:

```csharp
public class Person
{
  [AutoIncrement]
  public int Id { get; set; }

  [Reference]
  public Enterprise Enterprise { get; set; }
  public int EnterpriseId { get; set; }

  [StringLength(StringLengthAttribute.MaxText)]
  public string Name { get; set; }

  [Reference]
  public List<Contact> Contacts { get; set; }
}
```

## Resultat

Til slutt sitter jeg igjen med en ny struktur i prosjektet, noe basert på [denne](https://docs.servicestack.net/physical-project-structure). Hvor det nye endepunktet ligger under: People.Services/PeopleContactService.cs

Det kan testes ved å bruke http://localhost:61019/people/1/contacts. Denne henter ut alle kontaktene til den personen. Returnert JSON skjema:

```json
[
  {
    "id": 1,
    "personId": 1,
    "contactPerson": {
      "id": 2,
      "enterpriseId": 2,
      "name": "Kim Nerli"
    },
    "contactPersonId": 2
  },
  {
    "id": 2,
    "personId": 1,
    "contactPerson": {
      "id": 4,
      "enterpriseId": 3,
      "name": "Steve Balmer"
    },
    "contactPersonId": 4
  }
]
```
