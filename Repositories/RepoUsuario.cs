using devchat3.Data;
using devchat3.Models;
using devchat3.Repositories.Interfaces;

namespace devchat3.Repositories
{
    public class RepoUsuario : RepoGeneric<Usuario>, IRepoUsuario
    {
        public RepoUsuario(Context context) : base(context)
        {
        }

        public bool ExisteEmail(string email)
        {
            return context.Usuarios.Any(a => a.email == email);
        }
        public bool ExisteUsername(string userName)
        {
            return context.Usuarios.Any(a => a.userName == userName);
        }

        public Usuario Get(string dato)
        {
           return context.Usuarios.FirstOrDefault(x => x.email == dato || x.userName == dato);

        }
    }
}
