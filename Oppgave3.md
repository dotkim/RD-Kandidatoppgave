# Oppgave 3

## Hvordan ville du løst å legge til data?

For å legge til datta ville jeg laget en ny Route for en POST request.

## Hvordan kunne tabellene vært opprettet/dataene hentet ut på en alternativ måte?

Slik som jeg ser det ut fra hvis man fortsatt skal bruke ORMLite, er det 3 måter å opprette tabeller/hente ut data.

### Metode 1

Bruke Plain Old CLR Objects for å skape tabeller som da brukes som data typer ved uthenting. Dette er hva som er brukt i den koden jeg har skrevet.

### Metode 2

Bruke datatyper likt som en POCO, men bruke "custom Select expressions". Eksempel:

```csharp
public class Person
{
  public int Id { get; set; }
  public string Name { get; set; }
}

// Dette er litt pseudokode, så jeg er ikke 100% sikker på om dette virker.
public class GetPerson(int id)
{
  var query = db.From<Person>();
  return db.Select<Person>(query, p => p.Id == id);
}
```

### Metode 3

Den siste metoden blir nok å bruke strongly typed sql. Ved bruk av metoder som `Select` og `CustomJoin`. Eksempel:

```csharp
[Alias("People")]
public class Person
{
  public int Id { get; set; }
  public string Name { get; set; }
}

public class GetPerson(int id)
{
  return db.Select<Person>("SELECT Id, Name FROM People WHERE Id = @id", id);
}

public class Contact
{
  public int Id { get; set; }
  public int PersonId { get; set; }
  public string PersonName { get; set; }
  public int ContactPersonId { get; set; }
  Public string ContactPersonName { get; set; }
}

// Denne vil returnere en liste med kontaker, men den vil ikke ha med PersonId eller ContactPersonId, disse er ikke med i selecten.
public List<Contact> LoadContacts(IDbConnectionFactory dbFactory, int Id)
{
  using (var db = dbFactory.Open())
  {
    var q = db.From<Contact>();
    q.CustomJoin<Person>("LEFT JOIN People AS p1 ON Contact.PersonId = p1.Id");
    q.CustomJoin<Person>("LEFT JOIN People AS p2 ON Contact.ContactPersonId = p2.Id");
    q.Where(x => x.PersonId == Id);
    q.Select(@"Contact.Id,
      Person.Name AS PersonName,
      ContactPerson.Name AS ContactPersonName
    ");
    return db.Select<Contact>(q);
  }
}
```

## Videre forbedring av koden?

Hvis jeg ville gjort noe mer nå, så ville jeg nok splittet prosjektet ut fra hva som er anbefalt på [denne](https://docs.servicestack.net/physical-project-structure) siden. Jeg kunne godt også forklart 

## Feilhåndtering


## Beskrivelse av commit(s)

