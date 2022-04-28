namespace PrimerProyectoClubDeportivoPA2.Web.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PrimerProyectoClubDeportivoPA2.Web.Data.Entities;
    using System.Linq;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<AdditionalSkill> AdditionalSkills { get; set; }
        public DbSet<Admin> Admins { get; set; }      
        public DbSet<Agenda> Agendas { get; set; }        
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<WeekDay> WeekDays { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var cascadeFKs = builder.Model
           .G­etEntityTypes()
           .SelectMany(t => t.GetForeignKeys())
           .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restr­ict;
            }

            base.OnModelCreating(builder);
        }

    }
}
