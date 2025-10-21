using BlogApp.Data;
using BlogApp.Entidades;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Services
{
    public class BlogService
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogService(ApplicationDbContext context, AuthenticationStateProvider authenticationStateProvider, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _authenticationStateProvider = authenticationStateProvider;
            _userManager = userManager;
        }

        public async Task<List<BlogPost>> GetPublishedPostsAsync()
        {
            return await _context.BlogPosts
               .Where(p => p.IsPublished)
               .Include(p => p.Usuario)
               .Include(p => p.Categoria)
               .Include(p => p.FeaturedImage)
               .OrderByDescending(p => p.PublishedOn)
               .ToListAsync();
        }

        public async Task<List<Categoria>> GetCategoriesAsync()
        {
            return await _context.Categorias.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<List<BlogPost>> GetPublishedPostsByCategoryAsync(string categorySlug)
        {
            var category = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Slug == categorySlug);

            if (category == null)
            {
                return new List<BlogPost>();
            }

            return await _context.BlogPosts
                .Where(p => p.IsPublished && p.CategoryId == category.Id)
                .Include(p => p.Usuario)
                .Include(p => p.Categoria)
                .Include(p => p.FeaturedImage)
                .OrderByDescending(p => p.PublishedOn)
                .ToListAsync();
        }

        public async Task<BlogPost?> GetPostBySlugAsync(string slug)
        {
            return await _context.BlogPosts
               .Include(p => p.Usuario)
               .Include(p => p.Categoria)
               .Include(p => p.FeaturedImage)
               .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
        }

        public async Task<List<BlogPost>> GetPostsByUserAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            return await _context.BlogPosts
               .Where(p => p.UserId == currentUser.Id)
               .Include(p => p.Usuario)
               .Include(p => p.FeaturedImage)
               .OrderByDescending(p => p.CreatedOn)
               .ToListAsync();
        }

        public async Task<BlogPost?> GetPostForEditAsync(int postId)
        {
            var currentUser = await GetCurrentUserAsync();

            var post = await _context.BlogPosts
                .Include(p => p.FeaturedImage)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null && post.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("No tienes permiso para editar esta publicación.");
            }

            return post;
        }

        public async Task<BlogPost> CreatePostAsync(BlogPost post)
        {
            var currentUser = await GetCurrentUserAsync();
            post.UserId = currentUser.Id;
            post.CreatedOn = DateTime.UtcNow;

            if (post.IsPublished)
            {
                post.PublishedOn = DateTime.UtcNow;
            }
            else
            {
                post.PublishedOn = null;
            }

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<BlogPost> UpdatePostAsync(BlogPost updatedPost)
        {
            var existingPost = await _context.BlogPosts
                .Include(p => p.FeaturedImage)
                .FirstOrDefaultAsync(p => p.Id == updatedPost.Id);

            if (existingPost == null)
            {
                throw new KeyNotFoundException("La publicación no existe.");
            }

            var currentUser = await GetCurrentUserAsync();
            if (existingPost.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("No tienes permiso para editar esta publicación.");
            }

            existingPost.Title = updatedPost.Title;
            existingPost.Slug = updatedPost.Slug;
            existingPost.Introduction = updatedPost.Introduction;
            existingPost.Content = updatedPost.Content;
            existingPost.CategoryId = updatedPost.CategoryId;
            existingPost.ModifiedOn = DateTime.UtcNow;
            existingPost.FeaturedImageId = updatedPost.FeaturedImageId;

            if (updatedPost.IsPublished && !existingPost.IsPublished)
            {
                existingPost.PublishedOn = DateTime.UtcNow;
            }
            else if (!updatedPost.IsPublished)
            {
                existingPost.PublishedOn = null;
            }

            existingPost.IsPublished = updatedPost.IsPublished;

            await _context.SaveChangesAsync();
            return existingPost;
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            var postToDelete = await _context.BlogPosts.FindAsync(postId);
            if (postToDelete == null) return false;

            var currentUser = await GetCurrentUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (postToDelete.UserId != currentUser.Id && !isAdmin)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar esta publicación.");
            }

            _context.BlogPosts.Remove(postToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAllPostsByUserAsync(string userId)
        {
            var postsToDelete = await _context.BlogPosts
                .Where(p => p.UserId == userId)
                .ToListAsync();

            _context.BlogPosts.RemoveRange(postsToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Se requiere el rol Admin.");
            }

            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<string>> GetAllRoleNamesAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Acceso denegado.");
            }

            return await _context.Roles
                .Select(r => r.Name!)
                .Where(n => n != null)
                .ToListAsync();
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var existingRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = roles.Except(existingRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            var rolesToRemove = existingRoles.Except(roles);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return IdentityResult.Success;
        }

        public async Task<bool> DeleteCommentAsync(int commentId, string currentUserId)
        {
            var comment = await _context.Comentarios.FindAsync(commentId);
            if (comment == null) return false;

            var currentUser = await GetCurrentUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (comment.UserId != currentUserId && !isAdmin)
            {
                throw new UnauthorizedAccessException("No tienes permiso para eliminar este comentario.");
            }

            _context.Comentarios.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Comentario> UpdateCommentAsync(Comentario updatedComment, string currentUserId)
        {
            var existingComment = await _context.Comentarios.FindAsync(updatedComment.Id);

            if (existingComment == null)
            {
                throw new KeyNotFoundException("El comentario no existe.");
            }

            if (existingComment.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("No tienes permiso para editar este comentario.");
            }

            existingComment.Content = updatedComment.Content;
            existingComment.ModifiedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<List<Comentario>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comentarios
                .Where(c => c.BlogPostId == postId)
                .Include(c => c.Usuario)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task<Comentario> AddCommentAsync(Comentario comentario)
        {
            var currentUser = await GetCurrentUserAsync();

            comentario.UserId = currentUser.Id;

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            await _context.Entry(comentario).Reference(c => c.Usuario).LoadAsync();

            return comentario;
        }

        public async Task<Categoria> CreateCategoryAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> UpdateCategoryAsync(Categoria updatedCategoria)
        {
            _context.Update(updatedCategoria);
            await _context.SaveChangesAsync();
            return updatedCategoria;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryToDelete = await _context.Categorias.FindAsync(categoryId);
            if (categoryToDelete == null) return false;

            _context.Categorias.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var userPrincipal = authState.User;
            if (userPrincipal.Identity?.IsAuthenticated ?? false)
            {
                var user = await _userManager.GetUserAsync(userPrincipal);
                if (user != null) return user;
            }
            throw new InvalidOperationException("Usuario no autenticado.");
        }



        public async Task<List<BlogPost>> GetAllPostsForAdminAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isAdmin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Se requiere el rol Admin.");
            }

          
            return await _context.BlogPosts
                .Include(p => p.Usuario)
                .Include(p => p.Categoria)
                .Include(p => p.FeaturedImage)
                .OrderByDescending(p => p.CreatedOn)
                .ToListAsync();
        }

        public async Task DeleteAllCommentsByUserAsync(string userId)
        {
            var commentsToDelete = await _context.Comentarios
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.Comentarios.RemoveRange(commentsToDelete);
            await _context.SaveChangesAsync();
        }

       
        public async Task<bool> DeletePostByIdAsync(int postId)
        {
            var post = await _context.BlogPosts
                                   .Include(p => p.Comentarios) 
                                   .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return false;
            }

           
            _context.Comentarios.RemoveRange(post.Comentarios);
            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<List<BlogPost>> GetPostsByUserIdAsync(string userId)
        {
            return await _context.BlogPosts
                                 .Where(p => p.UserId == userId)
                                 .OrderByDescending(p => p.CreatedOn)
                                 .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
          
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<BlogPost?> GetPostByIdAsync(int postId)
        {
            
            return await _context.BlogPosts
               .Include(p => p.Usuario)
               .Include(p => p.Categoria)
               .Include(p => p.FeaturedImage)
               .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<List<BlogPost>> GetPostsWithCommentsByUserIdAsync(string userId)
        {
            return await _context.BlogPosts
                                 .Where(p => p.UserId == userId)
                                 
                                 .Include(p => p.Comentarios)
                                 .OrderByDescending(p => p.CreatedOn)
                                 .ToListAsync();
        }




   

    }
}