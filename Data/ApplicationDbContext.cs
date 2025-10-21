using BlogApp.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Usuario)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

         
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.BlogPost)
                .WithMany(b => b.Comentarios)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}