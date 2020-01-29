using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            CreateTablesAndTestData(dbFactory);
            SelectTestData(dbFactory);
        }

        private static void CreateTablesAndTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                db.ExecuteNonQuery("CREATE TABLE people (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL);");
                db.ExecuteNonQuery("INSERT INTO people (name) VALUES ('Willifred Manford');");
            }
        }

        private static void SelectTestData(IDbConnectionFactory dbFactory)
        {
            using (var db = dbFactory.Open())
            {
                var test = db.Select<string>("SELECT name FROM people");
            }
        }
    }
}
