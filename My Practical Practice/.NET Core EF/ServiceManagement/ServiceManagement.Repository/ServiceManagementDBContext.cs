using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServiceManagement.Core.Entity;
using ServiceManagement.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManagement.Repository
{
	public class ServiceManagementDBContext : DbContext
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ServiceManagementDBContext(DbContextOptions<ServiceManagementDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public DbSet<Service> Services { get; set; }
		public DbSet<ServiceBooking> ServicesBookings { get; set; }
		public DbSet<User> Users { get; set; }

		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Service>(entity =>
			{
				entity.HasKey(e => e.ServiceId);
				//Unique constraint on ServiceNumber.
				entity.HasIndex(e => e.ServiceNumber).IsUnique().HasDatabaseName("UK_ServiceNumber");
				entity.HasIndex(e => e.ServiceName).IsUnique().HasDatabaseName("UK_ServiceName");
				entity.Property(e => e.ServicePrice).IsRequired();
				//check constraint.
				entity.ToTable(t => t.HasCheckConstraint("CK_Duration", "[ServiceDuration] > 0"));
				entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
			});
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<ServiceBooking>(entity =>
			{
				entity.HasOne(d => d.Service)
					  .WithMany(p => p.ServiceBookings)
					  .HasForeignKey(d => d.ServiceID)
					  .OnDelete(DeleteBehavior.Cascade)
					  .HasConstraintName("FK_ServiceBooking_Service");
			});
		}


		/// <summary>
		/// Adds an entity asynchronously and saves changes to the database.
		/// </summary>
		public async Task<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
			where TEntity : class
		{
			// Note: EF Core's AddAsync handles special cases like HiLo value generators.
			await Set<TEntity>().AddAsync(entity, cancellationToken);
			await SaveChangesAsync(cancellationToken);
			return entity;
		}

		/// <summary>
		/// Updates an entity and saves changes asynchronously to the database.
		/// </summary>
		public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
			where TEntity : class
		{
			// Update in EF Core is an in-memory operation (attaches and sets EntityState.Modified)
			Set<TEntity>().Update(entity);
			await SaveChangesAsync(cancellationToken);
			return entity;
		}


		/// <summary>
		/// Override SaveChangesAsync to automatically set CreatedDate, CreatedBy, UpdatedDate, and UpdatedBy for entities that inherit from BaseModel.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			_httpContextAccessor.HttpContext.Items.TryGetValue("LoggedInUserId", out var loggedInUserIdObj);
			var loggedInUserId = loggedInUserIdObj != null ? Convert.ToInt64(loggedInUserIdObj) : 0;

			var entities = ChangeTracker.Entries()
				.Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

			foreach (var entry in entities)
			{
				var entity = (BaseModel)entry.Entity;

				if (entry.State == EntityState.Added)
				{
					// Set InsertedDate and InsertedBy when the entity is added
					entity.CreatedDate = DateTime.UtcNow;
					//  Set InsertedBy here if you have the user context
					entity.CreatedBy = (int)loggedInUserId;
				}
				else if (entry.State == EntityState.Modified)
				{
					// Update the UpdatedDate and UpdatedBy when the entity is modified
					entity.UpdatedDate = DateTime.UtcNow;
					// Set UpdatedBy here if you have the user context
					entity.UpdatedBy = (int)loggedInUserId;

					// Prevent updating InsertedDate and InsertedBy
					entry.Property("CreatedDate").IsModified = false;
					entry.Property("CreatedBy").IsModified = false;
				}
			}

			// Call the base SaveChangesAsync method
			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}
