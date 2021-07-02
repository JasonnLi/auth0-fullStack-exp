using System;
using AuthApp.Core.Exp.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// WbeApi is not using MVC middleware, so Models is the default directory to store your entity, you couldn't use Entity as your directory name to store your entity
// for Mac Nuget Package Console command, it maybe different from Windows, should be careful to use it
// e.g: get-help Add-Migration, get-help about_entityframeworkcore, Add-Migration InitialMigration, Update-Database
// When changing the db context, using migration first and then update the db

namespace AuthApp.Infra.Exp.Data
{
    public partial class AuthAppContext: DbContext
    {
        public AuthAppContext(DbContextOptions<AuthAppContext> options): base(options)
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }

        public virtual DbSet<AuthConnection> AuthConnection { get; set; }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<UserRole> UserRole { get; set; }

        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<RolePermission> RolePermission { get; set; }

        public virtual DbSet<Permission> Permission { get; set; }

        public virtual DbSet<Books> Books { get; set; }

        public virtual DbSet<BookGallery> BookGallery { get; set; }

        public virtual DbSet<Language> Language { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // The uuid-ossp module provides functions to generate universally unique identifiers (UUIDs)
            modelBuilder.HasPostgresExtension("uuid-ossp");

            ConfigureModel(modelBuilder.Entity<Customer>());
            ConfigureModel(modelBuilder.Entity<AuthConnection>());
            ConfigureModel(modelBuilder.Entity<User>());
            ConfigureModel(modelBuilder.Entity<Role>());
            ConfigureModel(modelBuilder.Entity<UserRole>());
            ConfigureModel(modelBuilder.Entity<Permission>());
            ConfigureModel(modelBuilder.Entity<RolePermission>());

            ConfigureModel(modelBuilder.Entity<Language>());
            ConfigureModel(modelBuilder.Entity<Books>());
            ConfigureModel(modelBuilder.Entity<BookGallery>());

            // hash data

            modelBuilder.Entity<Customer>().HasData(
                new Customer {
                    Id = 1,
                    PublicId = Guid.Parse("bbdee09c-089b-4d30-bece-44df5923716c"),
                    ApplicationId = "AuthApp",
                    EnvironmentType = "Dev",
                    OrgId = "EXP1",
                    Name = "Experiment1"
                },
                new Customer
                {
                    Id = 2,
                    PublicId = Guid.Parse("6fb600c1-9011-4fd7-9234-881379716440"),
                    ApplicationId = "AuthApp",
                    EnvironmentType = "Dev",
                    OrgId = "EXP2",
                    Name = "Experiment2"
                }
            );

            // Each customer should have at least one user account with ALM type
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    CustomerId = 1,
                    Email = "authappadmin@admin.com",
                    FirstName = "Jason",
                    LastName = "Lee",
                    Auth0Id = "auth0|5f36283c307dac0067919854",
                    Type = UserType.ALM,
                    Source = UserSource.IdentityProvider
                },
                new User
                {
                    Id = 2,
                    CustomerId = 2,
                    Email = "authappadmin1@admin.com",
                    FirstName = "admin1",
                    LastName = "admin1",
                    Auth0Id = "auth0|5f37c3dab0e72f006eb4cb53",
                    Type = UserType.ALM,
                    Source = UserSource.IdentityProvider
                }
            );

            modelBuilder.Entity<AuthConnection>().HasData(
                new AuthConnection
                {
                    Id = 1,
                    ApplicationId = "AuthApp",
                    EnvironmentType = "Dev",
                    CustomerId = 1,
                    ConnectionName = "Username-Password-Authentication"
                },
                new AuthConnection
                {
                    Id = 2,
                    ApplicationId = "AuthApp",
                    EnvironmentType = "Dev",
                    CustomerId = 1,
                    ConnectionName = "google-oauth2"
                },
                 new AuthConnection
                 {
                     Id = 3,
                     ApplicationId = "AuthApp",
                     EnvironmentType = "Dev",
                     CustomerId = 2,
                     ConnectionName = "Username-Password-Authentication"
                 },
                new AuthConnection
                {
                    Id = 4,
                    ApplicationId = "AuthApp",
                    EnvironmentType = "Dev",
                    CustomerId = 2,
                    ConnectionName = "google-oauth2"
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    CustomerId = 1
                },
                new Role
                {
                    Id = 2,
                    Name = "Standard",
                    CustomerId = 1
                },
                new Role
                {
                    Id = 3,
                    Name = "Admin",
                    CustomerId = 2
                },
                new Role
                {
                    Id = 4,
                    Name = "Standard",
                    CustomerId = 2
                }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = 1,
                    Action = "Create"
                },
                new Permission
                {
                    Id = 2,
                    Action = "Read"
                },
                new Permission
                {
                    Id = 3,
                    Action = "Update"
                },
                new Permission
                {
                    Id = 4,
                    Action = "Delete"
                }
            );

            modelBuilder.Entity<Language>().HasData(
                new Language
                {
                    Id = 1,
                    Name = "Chinese",
                    Description = "Originated from China"
                },
                new Language
                {
                    Id = 2,
                    Name = "English",
                    Description = "Popular in multiples countries"
                },
                new Language
                {
                    Id = 3,
                    Name = "French",
                    Description = "Originated from French"
                }
            );

        }

        private void ConfigureModel(EntityTypeBuilder<Customer> builder)
        {

            builder
                .Property(c => c.DateCreated)
                .HasDefaultValueSql("now()");

            builder
                .HasMany(c => c.Users)
                .WithOne(u => u.Customer);

            builder
                .HasMany(c => c.Roles)
                .WithOne(r => r.Customer);

            builder
                .HasMany(c => c.AuthConnections)
                .WithOne(a => a.Customer);

        }

        private void ConfigureModel(EntityTypeBuilder<AuthConnection> builder)
        {

            builder
                .Property(a => a.DateCreated)
                .HasDefaultValueSql("now()");

            builder
                .HasOne(a => a.Customer)
                .WithMany(c => c.AuthConnections)
                .HasForeignKey(a => a.CustomerId);

        }

        private void ConfigureModel(EntityTypeBuilder<User> builder)
        {

            builder
                .Property(u => u.DateCreated)
                .HasDefaultValueSql("now()");

            builder
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User);

            builder
                .HasMany(u => u.Books)
                .WithOne(b => b.User);

            builder
                .HasOne(u => u.Customer)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CustomerId);

        }

        private void ConfigureModel(EntityTypeBuilder<UserRole> builder)
        {

            builder
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // The relationship between UserRole and User, it has one user -> many roles, foreign key is UserId
            // In UserRole ur table, there is one User. In User u table, there are many UserRoles
            builder
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            builder
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

        }

        private void ConfigureModel(EntityTypeBuilder<Role> builder)
        {
            // HasMany A with one B is used in one relationship side
            // Role r has Many RoplePermission an in rp, it has one role
            builder
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role);

            builder
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role);

            builder
                .HasOne(r => r.Customer)
                .WithMany(c => c.Roles)
                .HasForeignKey(r => r.CustomerId);

        }

        private void ConfigureModel(EntityTypeBuilder<RolePermission> builder)
        {

            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            builder
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

        }

        private void ConfigureModel(EntityTypeBuilder<Permission> builder)
        {

            builder
                .HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission);

        }

        private void ConfigureModel(EntityTypeBuilder<Language> builder)
        {

            builder
                .HasMany(l => l.Books)
                .WithOne(b => b.Language);

        }

        private void ConfigureModel(EntityTypeBuilder<Books> builder)
        {

            builder
                .Property(b => b.CreateOn)
                .HasDefaultValueSql("now()");

            builder
                .HasOne(b => b.Language)
                .WithMany(l => l.Books)
                .HasForeignKey(b => b.LanguageId);

            builder
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId);

            builder
                .HasMany(b => b.bookGallery)
                .WithOne(bg => bg.Book);

        }

        private void ConfigureModel(EntityTypeBuilder<BookGallery> builder)
        {

            builder
                .HasOne(bg => bg.Book)
                .WithMany(b => b.bookGallery)
                .HasForeignKey(bg => bg.BookId);

        }

    }
}
