using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    public class Contact
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Person))]
        public int PersonId { get; set; }

        [References(typeof(Person))]
        public int ContactPersonId { get; set; }
    }

    [Alias("Contact")]
    public class ContactFull
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Person))]
        public string PersonName { get; set; }

        [References(typeof(Person))]
        public string ContactPersonName { get; set; }
    }
}