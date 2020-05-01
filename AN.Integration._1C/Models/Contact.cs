using System.ComponentModel.DataAnnotations;

namespace AN.Integration._1C.Models
{
    public class Contact: IOneCData
    {
        [MinLength(4, ErrorMessage = "Contact code must contain no less 4 symbols")]
        [MaxLength(4, ErrorMessage = "Contact code must contain no more 4 symbols")]
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
