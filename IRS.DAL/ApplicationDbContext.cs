using System;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Linq;
using IRS.DAL.ModelInterfaces;
using System.Threading.Tasks;
using System.Threading;

namespace IRS.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Roles, Guid,
        IdentityUserClaim<Guid>, UserRoles, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>

    {
        private IHttpContextAccessor _contextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor) : base(options)
        {
            _contextAccessor = contextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public Guid UserID
        {
            get
            {
                var userID = Guid.Empty;
                Guid.TryParse(_contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out userID);
                return userID;
            }
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddLoggingData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Media> Gallery { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Incidence> Incidences { get; set; }
        public DbSet<Hazard> Hazards { get; set; }
        public DbSet<IncidenceStatus> IncidenceStatuses { get; set; }
        public DbSet<IncidenceType> IncidenceTypes { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<State> States { get; set; }
        //public DbSet<Video> Videos { get; set; }

        public DbSet<OrganizationDepartment> OrganizationDepartments { get; set; }
        public DbSet<UserDeployment> UserDeployment { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

        public DbSet<IncidenceTypeDepartmentMapping> IncidenceTypeDepartmentMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Incidence>(e =>
            {
                e.HasOne(r => r.Assigner).WithMany(u => u.AllocatedIncidences).HasForeignKey(r => r.AssignerId);
                e.HasOne(r => r.Assignee).WithMany(u => u.AssignedIncidences).HasForeignKey(r => r.AssigneeId);
                e.HasOne(r => r.Organization).WithMany(u => u.OrganizationIncidences).HasForeignKey(r => r.OrganizationId);
                e.HasOne(r => r.AssignedOrganization).WithMany(u => u.AllocatedIncidences).HasForeignKey(r => r.AssignedOrganizationId);
            });

            //modelBuilder.Entity<Incidence>(e =>
            //{
            //    e.HasOne(r => r.AssignedDepartment).WithMany(u => u.AssignedDepartmentIncidences).HasForeignKey(r => r.AssignedDepartmentId);
            //    e.HasOne(r => r.ReportersDepartment).WithMany(u => u.ReporterDepartmentIncidences).HasForeignKey(r => r.ReporterDepartmentId);
            //});

            modelBuilder.Entity<Incidence>()
            .HasOne(e => e.CreatedByUser)
            .WithMany(c => c.CreatedIncidences);

            modelBuilder.Entity<Incidence>()
            .HasOne(e => e.EditedByUser)
            .WithMany(c => c.EditedIncidences);

            modelBuilder.Entity<User>()
            .HasOne(e => e.UserOrganization)
            .WithMany(c => c.Users);

            modelBuilder.Entity<UserRoles>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

        }

        protected virtual void AddLoggingData()
        {
            var entites = ChangeTracker.Entries().Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified) && (x.Entity is ICreateLoggable || x.Entity is IEditLoggable));
            var userID = UserID;
            foreach (var entity in entites)
            {
                var currentTime = DateTime.Now;
                if (entity.State == EntityState.Added)
                {
                    if (entity.Entity is ICreateLoggable)
                    {
                        var t = entity.Entity as ICreateLoggable;
                        if (t != null)
                        {
                            t.CreatedByUserId = userID;
                            t.DateCreated = currentTime;
                        }
                    }
                }
                else if (entity.State == EntityState.Modified)
                {
                    var t = entity.Entity as IEditLoggable;
                    if (t != null)
                    {
                        t.EditedByUserId = userID;
                        t.DateEdited = currentTime;
                    }
                }
            }
        }
    }
}
