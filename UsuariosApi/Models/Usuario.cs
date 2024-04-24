using Microsoft.AspNetCore.Identity;

namespace UsuariosApi.Models;

public class Usuario : IdentityUser // IdentityUser é uma classe do Identity que tem tudo pra cadastro de usuario
{
    public DateTime DataNascimento { get; set; }
    public Usuario() : base() { }
}

