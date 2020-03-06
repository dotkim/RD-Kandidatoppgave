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
}
