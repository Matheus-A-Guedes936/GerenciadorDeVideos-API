using GerenciadorDeVideos_API.DTOs.Usuarios;
using GerenciadorDeVideos_API.Interface.IRepository;
using GerenciadorDeVideos_API.Interface.IServices;
using GerenciadorDeVideos_API.Model;

namespace GerenciadorDeVideos_API.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<UsuarioRespostaDto>> ObterTodos()
        {
            var usuariosModel = await _usuarioRepository.ObterTodosUsuarios();

            return MapeamentoTodosParaDto(usuariosModel);
        }

        public async Task<UsuarioRespostaDto> ObterUsuarioPorId(int id)
        {
            var usuarioModel = await _usuarioRepository.ObterUsuarioPorId(id);

            if (usuarioModel == null) return null;

            return MapeamentoParaDto(usuarioModel);
        }

        public async Task<UsuarioRespostaDto> ObterUsuarioPorEmail(string email)
        {
            var usuarioModel = await _usuarioRepository.ObterUsuarioPorEmail(email);

            if (usuarioModel != null) return MapeamentoParaDto(usuarioModel);

            return null;
        }

        public async Task<IEnumerable<UsuarioRespostaDto>> ObterUsuarioPorNome(string nome)
        {
            var usuariosModel = await _usuarioRepository.ObterUsuarioPorNome(nome);

            return MapeamentoTodosParaDto(usuariosModel);
            
        }

        public async Task<UsuarioRespostaDto> AdicionarUsuario(UsuarioCadastroDto cadastroDto)
        {
            var verificacaoDeEmail = await ObterUsuarioPorEmail(cadastroDto.Email);

            if (verificacaoDeEmail != null) return null;

            UsuarioModel usuarioModel = new UsuarioModel
            {
                Nome = cadastroDto.Nome,
                Email = cadastroDto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(cadastroDto.Senha)
            };

            var usuario = await _usuarioRepository.AdicionarUsuario(usuarioModel);

            return MapeamentoParaDto(usuario);

        }

        public async Task<UsuarioRespostaDto> EditarUsuario(int id, UsuarioAtualizacaoDto usuarioAtualizacaoDto)
        {
            var usuarioModel = await _usuarioRepository.ObterUsuarioPorId(id);

            if (usuarioModel == null) return null;

            if(!string.IsNullOrWhiteSpace(usuarioAtualizacaoDto.Nome))
            {
                usuarioModel.Nome = usuarioAtualizacaoDto.Nome;
            }

            if (!string.IsNullOrWhiteSpace(usuarioAtualizacaoDto.Email))
            {
                usuarioModel.Email = usuarioAtualizacaoDto.Email;
            }

            if(!string.IsNullOrWhiteSpace(usuarioAtualizacaoDto.SenhaAtual))
            {
                usuarioModel.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizacaoDto.SenhaAtual);
            }

            await _usuarioRepository.EditarUsuario(usuarioModel);

            return MapeamentoParaDto(usuarioModel);

        }

        public async Task<bool> RemoverUsuario(int id)
        {
            var usuarioModel = await _usuarioRepository.ObterUsuarioPorId(id);

            if (usuarioModel == null) return false;

            await _usuarioRepository.RemoverUsuario(usuarioModel);

            return true;

        }


        private UsuarioRespostaDto MapeamentoParaDto(UsuarioModel usuarioModel)
        {
            return new UsuarioRespostaDto
            {
                Id = usuarioModel.Id,
                Nome = usuarioModel.Nome,
                Email = usuarioModel.Email,
            };
        }

        private IEnumerable<UsuarioRespostaDto> MapeamentoTodosParaDto(List<UsuarioModel> usuariosModel)
        {
            return usuariosModel.Select(u => MapeamentoParaDto(u));
        }
    }
}
