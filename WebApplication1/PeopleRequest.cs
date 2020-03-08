using ServiceStack;
using System.Collections.Generic;

namespace WebApplication1
{
    [Route("/people", "GET")]
    public class GetPeople : IReturn<List<Person>>
    {
    }

    [Route("/people/{Id}", "GET")]
    public class SearchPeople : IReturn<Person>
    {
        public int Id { get; set; }
    }

    [Route("/people/{Id}/contacts", "GET")]
    public class GetContacts : IReturn<List<Contact>>
    {
        public int Id { get; set; }
    }
}
