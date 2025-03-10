using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Cinema.ViewModels
{
    public class ActorOpsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Bio is required.")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters.")]
        public string Bio { get; set; }

        public string? ProfilePicture { get; set; } 

        [StringLength(300, ErrorMessage = "News cannot exceed 300 characters.")]
        public string News { get; set; }

        
        public IFormFile? Image { get; set; }
    }
}
