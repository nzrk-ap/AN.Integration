using System;
using Dapper.Contrib.Extensions;

namespace AN.Integration.Database.Models
{
    public class Contact: IDatabaseTable
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
