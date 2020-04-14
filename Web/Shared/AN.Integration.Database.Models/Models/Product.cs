using System;
using Dapper.Contrib.Extensions;

namespace AN.Integration.Database.Models.Models
{
    [Table("Products")]
    public class Product : IDatabaseTable
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
    }
}
