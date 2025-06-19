
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using ConfigurationsEntities.Entites;
using ConfigurationsEntities.Enum;

namespace MyHospialoo.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{

	

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{

		}
		public DbSet<Department> Departments { get; set; }
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Nurse> Nurses { get; set; }
		public DbSet<Surgery> Surgeries { get; set; }

		public DbSet<Massage> Massages { get; set; }

		public DbSet<Patient> Patients { get; set; }
		public DbSet<Secreetarie> Secreetaries { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Apply configurations from the assembly
			builder.ApplyConfigurationsFromAssembly(typeof(Department).Assembly);

			


			builder.Entity<UserTp>()
				.Property(x => x.PersonalTypes)
				.HasConversion(
					x => x.ToString(),
					x => (PersonalTypes)Enum.Parse(typeof(PersonalTypes), x))
				.HasMaxLength(50);

			builder.Entity<UserTp>()
				.ToTable("Users_Types");

			builder.Entity<Doctor>()
				.ToTable("Doctors");

			builder.Entity<Nurse>()
				.ToTable("Nurses");

			builder.Entity<Patient>()
				.ToTable("Patients");

            builder.Entity<Massage>()
                .ToTable("Massages");

            builder.Entity<Secreetarie>()
				.ToTable("Secreetaries");

			builder.Entity<UserTp>().UseTptMappingStrategy();


		}


	}
}
