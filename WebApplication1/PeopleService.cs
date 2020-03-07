using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Data;

namespace WebApplication1
{
    public class PeopleService : Service
    {
        private readonly IDatabase _database;
        private readonly IDbConnectionFactory _dbFactory;

        public PeopleService(IDatabase database, IDbConnectionFactory dbFactory)
        {
            _database = database;
            _dbFactory = dbFactory;
        }

        public List<Person> Get(GetPeople request)
        {
            var result = _database.GetPeople(_dbFactory);
            return result;
        }

        public Person Get(SearchPeople request)
        {
            var result = _database.LoadPersonById(_dbFactory, request.Id);
            return result;
        }

        public List<ContactFull> Get(GetContacts request)
        {
            var result = _database.LoadContacts(_dbFactory, request.Id);
            return result;
        }
    }
}
