using Microsoft.EntityFrameworkCore;

namespace BeltExam.Models
{
    public class ActivitiesContext : DbContext
    {
        public ActivitiesContext(DbContextOptions<ActivitiesContext> options) : base(options) {}
        public DbSet<User> Users {get;set;}
        public DbSet<Activity> Activities {get;set;}
        public DbSet<Participant> Participants {get;set;}
    }
}