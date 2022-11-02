using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Entities.Models
{
    public class LoginModel
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
