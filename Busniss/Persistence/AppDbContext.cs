using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace Busniss.Persistence;
public class AppDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    #region Poll Servay

    public DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        foreach (var entity in entries)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (entity.State == EntityState.Added)
            {
                entity.Property(e => e.CreatedById).CurrentValue = currentUserId;
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Property(e => e.UpdatedById).CurrentValue = currentUserId;
                entity.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region EF Course
    //public DbSet<Category> Categories { get; set; }
    //public DbSet<Blog> Blogs { get; set; }
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    //modelBuilder.Entity<Student>().ToTable("Students", schema: "dbo");
    //    //modelBuilder.Entity<Student>().Property(s => s.DateOfBirth).HasColumnName("Date Of Birth");
    //    ////modelBuilder.Ignore(typeof(Student));
    //    ////modelBuilder.Entity<Student>().Ignore(s => s.Name);
    //    //modelBuilder.Entity<Student>(st =>
    //    //{
    //    //    st.Property(s => s.Name).HasColumnType("nvarchar(50)");
    //    //    st.Property(s => s.DateOfBirth).HasColumnType("datetime2");
    //    //});
    //    ////modelBuilder.Entity<Student>().HasKey(s=>s.Id).HasName("test");
    //    //modelBuilder.Entity<Student>().Property(s => s.DateOfBirth).HasDefaultValue(DateTime.Now);
    //    //modelBuilder.Entity<Student>().Property(s => s.DateOfBirth).HasDefaultValueSql("GRTDATE()");
    //    //modelBuilder.Entity<Student>().Property(s => s.Email).HasComputedColumnSql("[Name] +'@gmail.com'");

    //    //modelBuilder.Entity<Category>().Property(c=>c.Id).ValueGeneratedOnAdd();
    //    modelBuilder.Entity<Post>().HasOne<Blog>().WithMany().HasForeignKey(p => p.BlogKey).HasConstraintName("FK_Plog_Post");

    //    //modelBuilder.Entity<Commints>()
    //    //    .HasOne(c => c.Blog)
    //    //    .WithMany(b => b.Commints)
    //    //    //.HasForeignKey(c=>c.BlogUrl) --> optional
    //    //    .HasPrincipalKey(b => b.url);
    //    modelBuilder.Entity<Blog>().HasMany(b => b.Commints).WithOne(c => c.Blog).HasPrincipalKey(b => new { b.url, b.Id });
    //    modelBuilder.Entity<Commints>().HasKey(c => new { c.Id, c.CommentUser });
    //    #region Many-To_Many

    //    #region With Navigation Prop
    //    //modelBuilder.Entity<Post>()
    //    //    .HasMany(p => p.Tags)
    //    //    .WithMany(t => t.Posts)
    //    //    .UsingEntity(j=>j.ToTable("PostTagsTests")); 
    //    #endregion


    //    #region No Navigation Prop

    //    //modelBuilder.Entity<Post>()
    //    //    .HasMany<Tag>()
    //    //    .WithMany()
    //    //    .UsingEntity(j=>j.ToTable("PostTags")); 
    //    #endregion
    //    #region Manually

    //    #region Without navigation prop in Parents model
    //    modelBuilder.Entity<PostTag>()
    //  .HasKey(pt => new { pt.PostId, pt.TagId });

    //    modelBuilder.Entity<PostTag>()
    //        .Property(pt => pt.PostedAt)
    //        .HasDefaultValueSql("GETDATE()");
    //    #endregion

    //    #region With navigation prop in parents model
    //    //modelBuilder.Entity<Post>()
    //    //     .HasMany(p => p.Tags)
    //    //     .WithMany(t => t.Posts)
    //    //     .UsingEntity<PostTag>(
    //    //        j => j.HasOne(pt => pt.Tag)
    //    //                .WithMany(p => p.PostTags)
    //    //                .HasForeignKey(pt => pt.TagId),
    //    //         j => j.HasOne(pt => pt.Post)
    //    //                .WithMany(p => p.PostTags)
    //    //                .HasForeignKey(pt => pt.PostId),

    //    //          j =>
    //    //          {
    //    //              j.HasKey(pt => new { pt.PostId, pt.TagId });
    //    //              j.Property(pt => pt.PostedAt).HasDefaultValueSql("GETDATE()");
    //    //          }
    //    //    ); 
    //    #endregion

    //    #endregion
    //    #endregion
    //    modelBuilder.Entity<Post>()
    //        .HasIndex(Post => new { Post.Title, Post.Contect })
    //        .IsUnique()
    //        .HasDatabaseName("IX_Title_Context_Test")
    //        .HasFilter("");


    //    base.OnModelCreating(modelBuilder);
    //} 
    #endregion

}
