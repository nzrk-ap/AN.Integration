using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AN.Integration.Database.Models.Models
{
    [Table("Products", Schema = "dbo")]
    public class Product : IDatabaseTable
    {
        [Key]
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
    }
}
