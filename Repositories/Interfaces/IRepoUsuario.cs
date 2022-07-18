using devchat3.Models;

namespace devchat3.Repositories.Interfaces
{
    public interface IRepoUsuario : IRepoGeneric<Usuario>
    {
        bool ExisteEmail(string email);
        bool ExisteUsername(string email);
        Usuario Get(string dato);
    }
    
}
