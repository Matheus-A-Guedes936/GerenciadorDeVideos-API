using GerenciadorDeVideos_API.Model;

namespace GerenciadorDeVideos_API.Interface
{
    public interface IVideosRepository
    {
        Task<List<VideosModel>> BusacarTodosVideos();
        Task<VideosModel?> BuscarVideoPorId(int videoID);
        Task<List<VideosModel>> BuscarVideosPorTitulo(string videoNome);
        Task<List<VideosModel>> BuscarVideosPorCategoria(string categoria);
        Task<List<VideosModel>> BuscarVideoPorUsuarioID(int usuarioID);
        Task<VideosModel> AdicionarVideo(VideosModel video);
        Task<VideosModel> EditarVideo(VideosModel video);
        Task RemoverVideo(VideosModel video);
    }
}
