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
                db.CreateTable<Enterprise>();
                db.CreateTable<Person>();

                //Insert data as shown in the "ExecuteNonQuery" below.
                var enterprises = new List<Enterprise>
                {
                    new Enterprise { Name = "Generic Enterprises" }
                };
                enterprises.ForEach((e) => db.Save(e));

                var people = new List<Person>
                {
                    new Person { Name = "Willifed Manford", Enterprise = new Enterprise { Name = "ACME Inc" } },
                    new Person { Name = "Kim Nerli", Enterprise = new Enterprise { Name = "BnL" } }
                };
                people.ForEach((p) => db.Save(p, references: true));
            }
        }

        public List<Person> ExampleData(IDbConnectionFactory dbFactory)
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
    }
}
