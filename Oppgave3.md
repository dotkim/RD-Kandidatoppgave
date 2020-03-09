# Oppgave 3

## Hvordan ville du løst å legge til data?

For å legge til datta ville jeg laget en ny Route for POST requests. Hvor APIet tar i mot en request body i JSON. Noe som dette:

```csharp
public PeopleService : Service
{
  // Si at body er en JSON string, { Name = "Kim" }.
  Post(CreatePerson request)
  {
    // Sjekk at alle verdier blir riktig, deretter Insert.
    Person newPerson = JSON.Deserialize<Person>(request.body);
    return db.CreatePerson(_dbFactory, newPerson);
  }
}

public CreatePerson(DbFactory factory, Person newPerson)
{
  return db.Insert<Person>(newPerson);
}
```

## Hvordan kunne tabellene vært opprettet/dataene hentet ut på en alternativ måte?

Slik som jeg ser det ut fra hvis man fortsatt skal bruke ORMLite, har jeg laget 3 metoder man kan hente ut data på, ellers blir det å lage tabeller likt som jeg har gjort i [denne](https://github.com/dotkim/RD-Kandidatoppgave/blob/oppg2-contacts/HelpFiles/DatabaseConceptDesign.sql) sql filen.

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

Ville lagt inn testing, noe jeg ikke er veldig god på men har brukt noe Mocha & Chai i Node.JS. Hvis jeg skulle gjort noe mer etter det, så ville jeg nok splittet prosjektet ut fra hva som er anbefalt på [denne](https://docs.servicestack.net/physical-project-structure) siden. Jeg kunne godt også forklart relasjonene litt bedre enten med kommentarer eller et diagram.

Om nødvendig kunne jeg også lagt til autentisering og skrive om servicen(e) til secure services for å bruke sessions.

## Feilhåndtering

For feilhåndtering ville jeg først laget noe som sender riktige http [statuskoder](https://docs.servicestack.net/error-handling#default-mapping-of-c-exceptions-to-http-errors). Eventuelt "Check" regler for hva man kan spørre om, likt som fra denne [lenken](https://docs.servicestack.net/design-message-based-apis#error-handling-and-validation).

## Beskrivelse av commit(s)

Når det kommer til commits prøver jeg å forklare så godt jeg kan hva som er gjort, derfor splitter jeg dem ut i flere når jeg først mener jeg har noe som fungerer. Hvis et repository krever spesielle formater på commits og pull requests følger jeg som regel de. Som f.eks. [reglene](https://github.com/accordproject/ergo/blob/master/DEVELOPERS.md#-git-commit-guidelines) til Ergo.

Her er alle commitene mine: https://github.com/dotkim/RD-Kandidatoppgave/commits/oppg2-contacts