namespace GerenciadorDeVideos_API.Model
{
    public class VideosModel
    {
        public UsuarioModel Usuario { get; set; }

        public int UsuarioID { get; set; }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string? CaminhoVideo { get; set; }
     }
}
