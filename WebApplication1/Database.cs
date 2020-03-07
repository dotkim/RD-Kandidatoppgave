using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public interface IDatabase
    {
        void CreateTablesAndTestData(IDbConnectionFactory dbFactory);

        List<PersonFull> GetPeople(IDbConnectionFactory dbFactory);
        PersonFull LoadPersonById(IDbConnectionFactory dbFactory, int Id);
        List<ContactFull> LoadContacts(IDbConnectionFactory dbFactory, int Id);
    }

    public class Database : IDatabase
    {
        public void CreateTablesAndTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                // Create tables in the correct order, because of the foreign key
                db.DropAndCreateTable<Enterprise>();
                db.DropAndCreateTable<Person>();
                db.DropAndCreateTable<Contact>();

                var enterprises = new List<Enterprise>
                {
                    new Enterprise { Name = "ACME Inc" },
                    new Enterprise { Name = "BnL" },
                    new Enterprise { Name = "Generic Enterprises" }
                };
                enterprises.ForEach(e => db.Save(e));

                // Create "empty" Person objects so we can link them as contacts.
                var people = new List<Person>
                {
                    new Person { Name = "Willifred Manford", EnterpriseId = 1 },
                    new Person { Name = "Kim Nerli", EnterpriseId = 2 },
                    new Person { Name = "Bill Gates", EnterpriseId = 3 },
                    new Person { Name = "Steve Balmer", EnterpriseId = 3 },
                    new Person { Name = "Finn Erik", EnterpriseId = 1 }
                };
                people.ForEach(p => db.Save(p));

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

        public List<PersonFull> GetPeople(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSelect<PersonFull>();
            }
        }

        public PersonFull LoadPersonById(IDbConnectionFactory dbFactory, int Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSingleById<PersonFull>(Id);
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
