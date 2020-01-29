using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Data;

namespace WebApplication1
{
    public class Endpoint : Service
    {
        private readonly IDatabase _database;
        private readonly IDbConnectionFactory _dbFactory;

        public Endpoint(IDatabase database, IDbConnectionFactory dbFactory)
        {
            _database = database;
            _dbFactory = dbFactory;
        }

        //Example endpoint
        public List<string> Get(DocumentRequest request)
        {
            var result = _database.ExampleData(_dbFactory);
            return result;
        }
    }
}
