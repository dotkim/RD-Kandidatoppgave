using System.Collections.Generic;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace WebApplication1
{
    public interface IDatabase
    {
        void CreateTablesAndTestData(IDbConnectionFactory dbFactory);

        List<string> ExampleData(IDbConnectionFactory dbFactory);
    }

    public class Database : IDatabase
    {
        public void CreateTablesAndTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                db.ExecuteNonQuery("CREATE TABLE people (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");
                db.ExecuteNonQuery("CREATE TABLE enterprises(id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");
                db.ExecuteNonQuery("INSERT INTO people (name) VALUES ('Willifred Manford');");
                db.ExecuteNonQuery("INSERT INTO enterprises (name) VALUES ('ACME Inc');");
                db.ExecuteNonQuery("INSERT INTO enterprises (name) VALUES ('Generic Enterprises');");
                db.ExecuteNonQuery("INSERT INTO enterprises (name) VALUES ('BnL');");
            }
        }

        public List<string> ExampleData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                return db.Select<string>("SELECT name FROM people");
            }
        }
    }
}
