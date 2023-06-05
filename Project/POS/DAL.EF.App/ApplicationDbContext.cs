using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<Business> Businesses { get; set; } = default!;
    public DbSet<BusinessCategory> BusinessCategories { get; set; } = default!;
    public DbSet<BusinessManager> BusinessManagers { get; set; } = default!;
    public DbSet<BusinessPicture> BusinessPictures { get; set; } = default!;
    public DbSet<Invoice> Invoices { get; set; } = default!;
    public DbSet<InvoiceRow> InvoiceRows { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderFeedback> OrderFeedbacks { get; set; } = default!;
    public DbSet<Picture> Pictures { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = default!;
    public DbSet<ProductPicture> ProductPictures { get; set; } = default!;
    public DbSet<Settlement> Settlements { get; set; } = default!;
    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

      //  // disable cascade delete
      //  foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
      //  {
      //      relationship.DeleteBehavior = DeleteBehavior.Restrict;
      //  }
//
      //  builder.Entity<Picture>()
      //      .HasMany(p => p.BusinessPictures)
      //      .WithOne(b => b.Picture)
      //      .OnDelete(DeleteBehavior.Cascade);
//
      //  builder.Entity<Picture>()
      //      .HasMany(p => p.ProductPictures)
      //      .WithOne(b => b.Picture)
      //      .OnDelete(DeleteBehavior.Cascade);
//
//
      //  builder.Entity<Order>()
      //      .HasOne(p => p.Invoice)
      //      .WithOne(b => b.Order)
      //      .OnDelete(DeleteBehavior.Cascade);
      //  
      //  builder.Entity<Order>()
      //      .HasOne(p => p.OrderFeedback)
      //      .WithOne(b => b.Order)
      //      .OnDelete(DeleteBehavior.Cascade);
      //  
      //  
      //  
      //  builder.Entity<Invoice>()
      //      .HasMany(p => p.InvoiceRows)
      //      .WithOne(b => b.Invoice)
      //      .OnDelete(DeleteBehavior.Cascade);
        
        
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);
        return base.SaveChangesAsync(cancellationToken);
    }


    // move all datetimes to UTC
    private void FixEntities(ApplicationDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties
                .Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value.ToUniversalTime(), DateTimeKind.Utc));
            }
        }
    }
}