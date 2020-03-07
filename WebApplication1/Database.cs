using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public interface IDatabase
    {
        void CreateTablesAndTestData(IDbConnectionFactory dbFactory);

        List<Person> GetPeople(IDbConnectionFactory dbFactory);
        Person LoadPersonById(IDbConnectionFactory dbFactory, int Id);
        List<ContactFull> LoadContacts(IDbConnectionFactory dbFactory, int Id);
    }

    public class Database : IDatabase
    {
        public void CreateTablesAndTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                // Create tables in the correct order, because of the foreign key
                db.DropAndCreateTable<Contact>();
                db.DropAndCreateTable<Person>();
                db.DropAndCreateTable<Enterprise>();

                // Create "empty" Person objects so we can link them as contacts.
                var people = new List<Person>
                {
                    new Person { Name = "Willifred Manford", Enterprise = new Enterprise { Name = "ACME Inc" } },
                    new Person { Name = "Kim Nerli",         Enterprise = new Enterprise { Name = "BnL" } },
                    new Person { Name = "Bill Gates",        Enterprise = new Enterprise { Name = "Generic Enterprises" } },
                    new Person { Name = "Steve Balmer",      Enterprise = db.SingleById<Enterprise>(3), EnterpriseId = 3 },
                    new Person { Name = "Finn Erik",         Enterprise = db.SingleById<Enterprise>(1), EnterpriseId = 1 }
                };
                people.ForEach(p => db.Save(p, references:true));

                var contacts = new List<Contact>
                {
                    new Contact { PersonId = 1, ContactPersonId = 2 },
                    new Contact { PersonId = 1, ContactPersonId = 4 },
                    new Contact { PersonId = 2, ContactPersonId = 5 },
                    new Contact { PersonId = 3, ContactPersonId = 4 },
                    new Contact { PersonId = 3, ContactPersonId = 1 },
                    new Contact { PersonId = 4, ContactPersonId = 3 },
                    new Contact { PersonId = 5, ContactPersonId = 1 },
                    new Contact { PersonId = 5, ContactPersonId = 2 }
                };
                contacts.ForEach(c => db.Save(c));
            }
        }

        public List<Person> GetPeople(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSelect<Person>();
            }
        }

        public Person LoadPersonById(IDbConnectionFactory dbFactory, int Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSingleById<Person>(Id);
            }
        }

        public List<ContactFull> LoadContacts(IDbConnectionFactory dbFactory, int Id)
        {
            using (var db = dbFactory.Open())
            {
                var q = db.From<Contact>();
                q.LeftJoin<Person>((c, p) => c.PersonId == p.Id, db.TableAlias("Person"));
                q.LeftJoin<Person>((c, p) => c.ContactPersonId == p.Id, db.TableAlias("ContactPerson"));
                q.Where(x => x.PersonId == Id);
                q.Select(@"Contact.Id,
                    Person.Name AS PersonName,
                    ContactPerson.Name AS ContactPersonName
                ");
                return db.Select<ContactFull>(q);
            }
        }
    }
}
