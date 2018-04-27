using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreApp.Models.CustomValidations;

namespace NetCoreApp.Models
{
    public class ActivitiesViewModel : BaseEntity
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must be a minimum of 2 characters long")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [InTheFuture(ErrorMessage = "Must be in the future")]
        [DisplayFormat(DataFormatString = "{0:MMM-dd-yyyy}")]
        public DateTime? Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime? Time { get; set; }


        [Required]
        // [DataType(DataType.Time)]
        // [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public string Duration { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Description must be a minimum of 10 characters")]
        public string Description { get; set; }

    }
}