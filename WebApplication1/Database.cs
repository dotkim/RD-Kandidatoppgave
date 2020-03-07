using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public interface IDatabase
    {
        void CreateTablesAndTestData(IDbConnectionFactory dbFactory);

        List<Person> ExampleData(IDbConnectionFactory dbFactory);
        Person LoadPersonById(IDbConnectionFactory dbFactory, int Id);
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

        {
            using (var db = dbFactory.Open())
            {
                return db.LoadSingleById<Person>(Id);
            }
        }
    }
}
