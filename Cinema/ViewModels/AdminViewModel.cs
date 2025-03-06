using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class AdminViewModel
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Conform Password should be same")]
        public string ConformPassword { get; set; }

        public string SelectedRole { get; set; }

       

    }
}
