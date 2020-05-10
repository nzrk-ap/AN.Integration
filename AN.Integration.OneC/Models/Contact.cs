using System.ComponentModel.DataAnnotations;

namespace AN.Integration.OneC.Models
{
    public class Contact: IOneCData
    {
        public Contact(string code)
        {
            Code = code;
        }

        [MinLength(4, ErrorMessage = "Contact code must contain no less 4 symbols")]
        [MaxLength(4, ErrorMessage = "Contact code must contain no more 4 symbols")]
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
