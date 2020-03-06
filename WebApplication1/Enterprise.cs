using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    public class Enterprise
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int PersonId { get; set; }

        [StringLength(StringLengthAttribute.MaxText)]
        public string Name { get; set; }
    }
}
