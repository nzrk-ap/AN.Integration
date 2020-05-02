using System.ComponentModel.DataAnnotations;

namespace AN.Integration.OneC.Models
{ 
   public class Product: IOneCData
    {
        [MinLength(4, ErrorMessage = "Product code must contain no less 8 symbols")]
        [MaxLength(4, ErrorMessage = "Product code must contain no more 8 symbols")]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
