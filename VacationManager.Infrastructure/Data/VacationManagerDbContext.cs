

using Microsoft.EntityFrameworkCore;
using VacationManager.Domain.Models;

namespace VacationManager.Infrastructure.Data
{
    public class VacationManagerDbContext : DbContext
    {
        #region Constructeur de la DatabaseContext
        public VacationManagerDbContext(DbContextOptions<VacationManagerDbContext> options) : base(options)
        { }
        #endregion

        #region Ajout des entités dans la base de données
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Vacations> Vacations { get; set; }
        public DbSet<VacationsBalance> VacationsBalances { get; set; }
        public DbSet<VacationsReport> VacationsReports { get; set; }
        public DbSet<VacationsCalendar> VacationsCalendars { get; set; }
        #endregion

        #region Configuration de la connexion à la base de données
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DELL-LATIDUDEE7;Database=VacationManagerDb;Trusted_Connection=True; Encrypt=False;");
        }
        #endregion

        #region Configuration de la structure des entités dans la base de données
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de l'entité Users
            modelBuilder.Entity<Users>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserVacations)
                .WithOne(v => v.Users)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasOne(u => u.Roles)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);

            // Configuration de l'entité Roles
            modelBuilder.Entity<Roles>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Roles>()
                .Property(r => r.Name)
                .HasMaxLength(255);

            modelBuilder.Entity<Roles>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Roles)
                .HasForeignKey(u => u.RoleID);


            // Configuration de l'entité Vacations
            modelBuilder.Entity<Vacations>()
                .HasKey(v => v.Id);


            modelBuilder.Entity<Vacations>()
                .HasOne(v => v.Users)
                .WithMany(u => u.UserVacations)
                .HasForeignKey(v => v.UserId);


            // Configuration de l'entité VacationsBalance
            modelBuilder.Entity<VacationsBalance>()
                .HasKey(vb => vb.Id);

            modelBuilder.Entity<VacationsBalance>()
                .HasOne(vb => vb.Users)
                .WithMany(u => u.UserVacationsBalances)
                .HasForeignKey(vb => vb.UserId);

            // Configuration de l'entité VacationsCalendar
            modelBuilder.Entity<VacationsCalendar>()
                .HasKey(vc => vc.Id);

            modelBuilder.Entity<VacationsCalendar>()
                .HasOne(vc => vc.Users)
                .WithMany(u => u.UserVacationsCalendars)
                .HasForeignKey(vc => vc.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VacationsCalendar>()
                .HasOne(vc => vc.Vacations)
                .WithMany(v => v.Calendars)
                .HasForeignKey(vc => vc.VacationId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuration de l'entité VacationsReport
            modelBuilder.Entity<VacationsReport>()
                .HasKey(vr => vr.Id);

            modelBuilder.Entity<VacationsReport>()
                .HasOne(vr => vr.Users)
                .WithMany(u => u.VacationsReports)
                .HasForeignKey(vr => vr.UserId);
        }
        #endregion
    }
}
