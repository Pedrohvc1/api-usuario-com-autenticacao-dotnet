using Microsoft.AspNetCore.Authorization;

namespace UsuariosApi.Authorizantion;

public class IdadeMinima: IAuthorizationRequirement
{
    public int Idade { get; set; }
    public IdadeMinima(int idade)
    {
        Idade = idade;
    }
}
