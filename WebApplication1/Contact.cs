using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    // Because of a limitation with ORMLite: https://github.com/ServiceStack/ServiceStack.OrmLite#single-primary-key
    // Thus the Id field cannot be ommited, and a composite unique constraint is added to ensure unique rows.
    [UniqueConstraint(nameof(PersonId), nameof(ContactPersonId))]
    public class Contact
    {
        [AutoIncrement]
        public int Id { get; set; }

        [References(typeof(Person))]
        public int PersonId { get; set; }

        [References(typeof(Person))]
        public Person ContactPerson { get; set; }
        public int ContactPersonId { get; set; }
    }
}