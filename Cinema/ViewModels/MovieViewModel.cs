using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Cinema.Models.enums;
using Cinema.Models;
using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Movie image is required.")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Trailer URL is required.")]
        public string? TrailerUrl { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }

        [Required(ErrorMessage = "Please select a cinema.")]
        public int CinemaId { get; set; }
        public List<ECinema>? Cinemas { get; set; }

        public List<int>? SelectedActorIds { get; set; } = new List<int>();
        public List<Actor>? Actors { get; set; } = new List<Actor>();
    }
}