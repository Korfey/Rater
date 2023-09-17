using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rater.Models;

namespace Rater.Data
{
    public class ApplicationDbContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewTag> ReviewTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureUser(builder);
            ConfigureComment(builder);
            ConfigureLike(builder);
            ConfigureRating(builder);
            ConfigureReview(builder);
            ConfigureReviewTag(builder);
            ConfigureTag(builder);
        }

        private static void ConfigureUser(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(u => u.UserName)
                .HasMaxLength(30);
        }

        private static void ConfigureComment(ModelBuilder builder)
        {
            builder.Entity<Comment>()
                .Property(c => c.Created)
                .HasDefaultValueSql("getdate()");
            
            builder.Entity<Comment>()
                .HasIndex(c=>c.Created);

            builder.Entity<Comment>()
                .Property(c => c.Text)
                .HasColumnType("nvarchar(max)");

            builder.Entity<Comment>()
                .HasOne(c=>c.Review)
                .WithMany(r=>r.Comments)
                .HasForeignKey(c=>c.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureLike(ModelBuilder builder)
        {
            builder.Entity<Like>()
                .HasKey(l => new { l.ReviewId, l.UserId });

            builder.Entity<Like>()
                .HasOne(l=>l.Review)
                .WithMany(r => r.Likes)
                .HasForeignKey(l => l.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureRating(ModelBuilder builder)
        {
            builder.Entity<Rating>()
                .HasKey(r => new { r.ReviewId, r.UserId });

            builder.Entity<Rating>()
                .HasOne(r=>r.Review)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r=>r.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureReview(ModelBuilder builder)
        {
            builder.Entity<Review>()
                .Property(r=>r.Created)
                .HasDefaultValueSql("getdate()");

            builder.Entity<Review>()
                .HasIndex(r => r.Created);

            builder.Entity<Review>()
                .Property(r => r.Title)
                .HasColumnType("nvarchar(255)");

            builder.Entity<Review>()
                .Property(r=>r.Group)
                .HasConversion(
                g => g.ToString(),
           g => (Group)Enum.Parse(typeof(Group), g));
        }

        private static void ConfigureReviewTag(ModelBuilder builder)
        {
            builder.Entity<ReviewTag>()
                .HasKey(rt => new { rt.ReviewId, rt.TagId });
        }

        private static void ConfigureTag(ModelBuilder builder)
        {
            builder.Entity<Tag>()
                .Property(t=>t.Name)
                .HasColumnType("nvarchar(50)");
        }
    }
}