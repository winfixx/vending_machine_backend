using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VendingMachine.Core.Interfaces.Auth;
using VendingMachine.Core.Models.Enum;
using VendingMachine.Core.Services;
using VendingMachine.DataAccess.Entites;

namespace VendingMachine.DataAccess
{
    public class VendingMachineDbContext : DbContext
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IConfiguration configuration;

        public VendingMachineDbContext(
            DbContextOptions<VendingMachineDbContext> options,
            IPasswordHasher passwordHasher,
            IConfiguration configuration)
            : base(options)
        {
            this.passwordHasher = passwordHasher;
            this.configuration = configuration;
            Database.EnsureCreated();
        }

        public DbSet<DrinkEntity> Drinks { get; set; }
        public DbSet<VendingMachineEntity> VendingMachine { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserEntity>()
                .HasData(new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Jojo",
                    PasswordHash = passwordHasher.Generate(configuration["ListAdmin:Jojo:Password"]),
                    Role = UserRole.Admin
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
