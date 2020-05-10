using System.ComponentModel.DataAnnotations;

namespace AN.Integration.OneC.Models
{
    public interface IOneCData
    {
        [Required]
        string Code { get; set; }
    }
}