using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class LoginUserData
    {

        [Required(ErrorMessage =" User Name is Required")]
        [Display(Name="User Name")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is Required")]
        [Display(Name = "Password")]
        public string Password{ get; set; }

        public bool RememberMe { get; set; }


    }


}
