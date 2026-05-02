using GerenciadorDeVideos_API.Model;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeVideos_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<VideosModel> Videos { get; set; }
    }
}
