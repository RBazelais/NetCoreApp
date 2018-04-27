using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApp.Models
{
    public class Participant 
    {
        [Key]
        public int ParticipantId { get; set; }
        
        [ForeignKey("Activities")]
        public int ActivityId { get; set; }
        public Activities Activities { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        
    }

}