using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApp.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Activities> Activities { get; set; } // 1:M

        public List<Participant> Participants { get; set; } // M:M
    
        public User()
        {
            Activities = new List<Activities>();
        }
    }

}