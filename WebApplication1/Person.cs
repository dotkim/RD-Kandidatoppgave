using ServiceStack.DataAnnotations;

namespace WebApplication1
{
    [Alias("People")]
    public class Person
    {
        [AutoIncrement]
        public int Id { get; set; }
        
        [Reference]
        public Enterprise Enterprise { get; set; }
        public int EnterpriseId { get; set; }

        [StringLength(StringLengthAttribute.MaxText)]
        public string Name { get; set; }
    }

}
