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

        /// <summary>
        /// HTTP GET request for fetching a list of all rows in the people table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of Person objects.</returns>
        public List<Person> Get(GetPeople request)
        {
            var result = _database.GetPeople(_dbFactory);
            return result;
        }

        /// <summary>
        /// HTTP GET request for fetching a single person from the people table.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A Person object.</returns>
        public Person Get(SearchPeople request)
        {
            var result = _database.LoadPersonById(_dbFactory, request.Id);
            return result;
        }
    }
}
