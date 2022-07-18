using devchat3.Data;
using devchat3.Repositories;
using devchat3.Repositories.Interfaces;

namespace devchat3.UOW
{
    public class UOW : IUOW
    {
        private readonly Context context;
        public IRepoUsuario Repousuario { get; set; }

        public UOW(Context context)
        {
            this.context = context;
            Repousuario = new RepoUsuario(context);
        }


        
        public void Dispose()
        {
            context.Dispose();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
