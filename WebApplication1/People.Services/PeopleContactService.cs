using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Data;


namespace WebApplication1
{
    public class PeopleContactService : Service
    {
        private readonly IDatabase _database;
        private readonly IDbConnectionFactory _dbFactory;

        public PeopleContactService(IDatabase database, IDbConnectionFactory dbFactory)
        {
            _database = database;
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// HTTP GET request for listing all the contacts from a single person.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of Contact objects.</returns>
        public List<Contact> Get(GetPeopleContacts request)
        {
            var result = _database.LoadContacts(_dbFactory, request.Id);
            return result;
        }
    }
}
