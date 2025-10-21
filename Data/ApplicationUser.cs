using BlogApp.Entidades;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogApp.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    
        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}