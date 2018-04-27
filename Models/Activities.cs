using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreApp.Models.CustomValidations;

namespace NetCoreApp.Models
{
    public class Activities : BaseEntity
    {
        [Key]
        public int ActivityId { get; set; }

        public string Title { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Time { get; set; }

        public string Duration { get; set; }

        public string Description { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


        public List<Participant> Participants { get; set; }
        public Activities()
        {
            // creates an instance of an empty list of Participant objects
            Participants = new List<Participant>();
        }
    }
}