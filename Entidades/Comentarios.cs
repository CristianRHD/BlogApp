using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogApp.Data; 
namespace BlogApp.Entidades
{
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public required string Content { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }


        public int BlogPostId { get; set; }
        public required string UserId { get; set; }

       
        [ForeignKey("BlogPostId")]
        public BlogPost? BlogPost { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? Usuario { get; set; }
    }
}