using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entidades
{

    [Index(nameof(Slug), IsUnique = true)]
    public class Categoria
    {

        [Key]
        
        public int Id { get; set; }

        [Required, MaxLength(100), Unicode(false)]
        public required string Name { get; set; }

        [Required, MaxLength(125), Unicode(false)]
        public required string Slug { get; set; }

        public Categoria Clone() => (Categoria)this.MemberwiseClone();
    }
}
