namespace Camino.Framework.Models
{
    public class BaseFilterModel
    {
        public BaseFilterModel()
        {
            Page = 1;
        }

        [GraphQLType(typeof(IntType))]
        public int Page { get; set; }
        [GraphQLType(typeof(IntType))]
        public int? PageSize { get; set; }
        [GraphQLType(typeof(StringType))]
        public string Search { get; set; }
    }
}
