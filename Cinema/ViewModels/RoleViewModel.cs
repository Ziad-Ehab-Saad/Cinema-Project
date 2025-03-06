using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role Name is required")]
        [Display(Name ="Role Name")]
        public string Name { get; set; }
    }
}
