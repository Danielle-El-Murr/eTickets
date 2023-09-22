using eTickets.Data.Base;
using eTickets.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTickets.Models
{
    public class NewMovieVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        [Display(Name="Movie name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [Display(Name = "Movie Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Display(Name = "Price in $")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Movie poster URL is Required")]
        [Display(Name = "Movie poster URL")]
        public string ImageURL { get; set; }

        [Required(ErrorMessage = "StartDate is Required")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is Required")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "MovieCategory is Required")]
        [Display(Name = "Select a Movie category")]
        public MovieCategory MovieCategory { get; set; }

        [Required(ErrorMessage = "Movie actor(s) is Required")]
        [Display(Name = "Select actors")]
        public List<int> ActorIds { get; set; }

        [Required(ErrorMessage = "Movie cinema is Required")]
        [Display(Name = "Select a cinema")]
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "Movie producer is Required")]
        [Display(Name = "Select a producer")]
        public int ProducerId { get; set; }


    }
}
