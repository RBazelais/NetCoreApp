
using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
 
namespace NetCoreApp.Models
{
    public class NetAppContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public NetAppContext(DbContextOptions<NetAppContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Activities> Activities { get; set; } // 1:M

        public DbSet<Participant> Participants { get; set; } // M:M

    }
}