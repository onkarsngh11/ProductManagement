using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace  ProductManagement.Entities.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
