using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    public class Person
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [StringLength(StringLengthAttribute.MaxText)]
        public string Name { get; set; }

        [Reference]
        public Enterprise Enterprise { get; set; }
    }
}
