using GerenciadorDeVideos_API.DTOs.Videos;
using GerenciadorDeVideos_API.Interface.IRepository;
using GerenciadorDeVideos_API.Interface.IServices;
using GerenciadorDeVideos_API.Model;
using GerenciadorDeVideos_API.Model.Enums;
using System.Runtime;

namespace GerenciadorDeVideos_API.Services
{
    public class VideosService : IVideosService
    {
        private readonly IVideosRepository _videosRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public VideosService(IVideosRepository videosRepository , IUsuarioRepository usuarioRepository)
        {
            _videosRepository = videosRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<VideosRespostaDto>> BuscarTodosVideos()
        {
            var videosModel = await _videosRepository.BuscarTodosVideos();

            return MapeamentoTodosParaDto(videosModel);
        }

        public async Task<VideosRespostaDto?> BuscarVideoPorId(int videoID)
        {
            var videoModel = await _videosRepository.BuscarVideoPorId(videoID);
            if (videoModel == null)
                return null;
            return MapeamentoParaDto(videoModel);
        }  

        public async Task<IEnumerable<VideosRespostaDto>> BuscarVideosPorTitulo(string titulo)
        {
            var videoModel = await _videosRepository.BuscarVideosPorTitulo(titulo);
            return MapeamentoTodosParaDto(videoModel);
        }

        public async Task<IEnumerable<VideosRespostaDto>> BuscarVideosPorCategoria(CategoriaVideo categoria)
        {
            var videoModel = await _videosRepository.BuscarVideosPorCategoria(categoria);
            return MapeamentoTodosParaDto(videoModel);
        }

        public async Task<IEnumerable<VideosRespostaDto>> BuscarVideoPorUsuarioID(int usuarioID)
        {
            var videoModel = await _videosRepository.BuscarVideoPorUsuarioID(usuarioID);
            return MapeamentoTodosParaDto(videoModel);
        }

        public async Task<VideosRespostaDto?> AdicionarVideo(VideosCriacaoDto adicionarVideoDto)
        {
            var usuarioExiste = await _usuarioRepository.ObterUsuarioPorId(adicionarVideoDto.UsuarioID);
            if (usuarioExiste == null)
            {
                throw new Exception($"O usuário com ID {adicionarVideoDto.UsuarioID} não existe no sistema.");
            }

            if (adicionarVideoDto.ArquivoVideo.Length < 1024) 
            {
                throw new Exception("O arquivo enviado é pequeno demais para ser um vídeo válido.");
            }

            var pastaUsuario = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "videos",
                adicionarVideoDto.UsuarioID.ToString());

            if (!Directory.Exists(pastaUsuario)){
                Directory.CreateDirectory(pastaUsuario);
            }

            var nomeArquivo = $"{Guid.NewGuid().ToString() + Path.GetExtension(adicionarVideoDto.ArquivoVideo.FileName)}";

            var caminhoCompleto = Path.Combine(pastaUsuario, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await adicionarVideoDto.ArquivoVideo.CopyToAsync(stream);
            }

            VideosModel videosModel = new VideosModel
            {
                Titulo = adicionarVideoDto.Titulo,
                Categoria = adicionarVideoDto.Categoria,
                UsuarioID = adicionarVideoDto.UsuarioID,
                CaminhoVideo = $"/videos/{adicionarVideoDto.UsuarioID}/{nomeArquivo}"
            };

            var videoCriado = await _videosRepository.AdicionarVideo(videosModel);

            return MapeamentoParaDto(videoCriado);
        }

        public async Task<VideosRespostaDto> EditarVideo(VideosAtualizacaoDto atualizarVideoDto, int videoID)
        {
            var videoExistente = await _videosRepository.BuscarVideoPorId(videoID);
            if (videoExistente == null) return null;

            if (!string.IsNullOrWhiteSpace(atualizarVideoDto.Titulo))
            {
                videoExistente.Titulo = atualizarVideoDto.Titulo;
            }

            videoExistente.Categoria = atualizarVideoDto.Categoria;

            var videoAtualizado = await _videosRepository.EditarVideo(videoExistente);
            return MapeamentoParaDto(videoAtualizado);
        }

        public async Task<bool> RemoverVideo(int videoID)
        {
            var videoExistente = await _videosRepository.BuscarVideoPorId(videoID);
            if (videoExistente == null) return false;

            try
            {
                var caminhoRelativo = videoExistente.CaminhoVideo.TrimStart('/');
                var caminhoCompleto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", caminhoRelativo);

                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
            }
            catch (IOException ex)
            {
                throw new Exception("Erro ao tentar excluir o arquivo de vídeo.", ex);
            }

            await _videosRepository.RemoverVideo(videoExistente);
            return true;
        }


        private IEnumerable<VideosRespostaDto> MapeamentoTodosParaDto(List<VideosModel> videos)
        {
            return videos.Select(video => MapeamentoParaDto(video)).ToList();
        }

        private VideosRespostaDto MapeamentoParaDto(VideosModel video)
        {
            return new VideosRespostaDto
            {
                Id = video.Id,
                Titulo = video.Titulo,
                Categoria = video.Categoria.ToString(),
                CaminhoVideo = video.CaminhoVideo,
                UsuarioID = video.UsuarioID
            }
            ;
        }
    }
}
