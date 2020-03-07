using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    public class Enterprise
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Unique]
        [StringLength(StringLengthAttribute.MaxText)]
        public string Name { get; set; }
    }
}