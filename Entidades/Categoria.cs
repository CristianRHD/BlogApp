using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Entidades
{

    [Index(nameof(Name), IsUnique = true)]
    public class Categoria
    {

        [Key]
        
        public int Id { get; set; }

        [Required, MaxLength(100), Unicode(false)]
        public required string Name { get; set; }

        public Categoria Clone() => (Categoria)this.MemberwiseClone();
    }
}
