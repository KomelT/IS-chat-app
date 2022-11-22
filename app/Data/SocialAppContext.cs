using D_real_social_app.Models;
using Microsoft.EntityFrameworkCore;

namespace D_real_social_app.Data
{
    public class SocialAppContext : DbContext
    {
        public SocialAppContext(DbContextOptions<SocialAppContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Connection> Connection { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Connection>().ToTable("Connection");
        }
    }
}