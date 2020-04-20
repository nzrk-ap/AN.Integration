using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AN.Integration.Database.Models.Models
{
    [Table("Contacts", Schema = "dbo")]
    public class Contact: IDatabaseTable
    {
        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        [ForeignKey("Accounts")]
        public Guid? AccountId { get; set; }
    }
}
