using GerenciadorDeVideos_API.Model;

namespace GerenciadorDeVideos_API.Interface.IServices
{
    public interface ITokenService
    {
        string GerarToken(UsuarioModel usuario);
    }
}
