using ServiceStack;
using System.Collections.Generic;

namespace WebApplication1
{
    [Route("/people/{Id}/contacts", "GET")]
    public class GetPeopleContacts : IReturn<List<Contact>>
    {
        public int Id { get; set; }
    }
}
