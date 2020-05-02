using System.ComponentModel.DataAnnotations;

namespace AN.Integration.OneC.Models
{
    public class Account
    {
        [StringLength(4, ErrorMessage = "Product code max length is exceeded")]
        public string Code { get; set; }

        public string Name { get; set; }
    }
}
