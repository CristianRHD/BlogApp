using BlogApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Entidades
{
    [Index(nameof(Slug), IsUnique = true)]
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public required string Title { get; set; }

        [Required, MaxLength(150)]
        public required string Slug { get; set; }


        public int? CategoryId { get; set; }


        public required string UserId { get; set; }

        [Required, MaxLength(250)]
        public required string Introduction { get; set; }


        public required string Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }


        public virtual Categoria? Categoria { get; set; }
        public virtual ApplicationUser? Usuario { get; set; }

        [NotMapped]
        public string? CategoryName => Categoria?.Name;


        public int? FeaturedImageId { get; set; }


        [ForeignKey("FeaturedImageId")]
        public MediaFile? FeaturedImage { get; set; }

       
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}