using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeVideos_API.DTOs.Usuarios
{
    public class UsuarioAtualizacaoDto
    {
        [MinLength(3, ErrorMessage = "O Nome De Usuario deve possuir ao menos 3 caracteres")]
        string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Email invalido")]
        string Email { get; set; }

        [MinLength(6, ErrorMessage = "A senha deve possuir ao menos 6 caracteres")]
        string SenhaAtual {  get; set; }
    }
}
