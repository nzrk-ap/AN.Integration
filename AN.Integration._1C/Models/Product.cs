using System.ComponentModel.DataAnnotations;
using AN.Integration._1C.Models;

namespace AN.Integration.Models._1C.Dto
{ 
   public class Product: IOneCData
    {
        [MinLength(4, ErrorMessage = "Product code must contain no less 8 symbols")]
        [MaxLength(4, ErrorMessage = "Product code must contain no more 8 symbols")]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
