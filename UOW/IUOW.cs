using devchat3.Repositories.Interfaces;

namespace devchat3.UOW
{
    public interface IUOW : IDisposable
    {
        IRepoUsuario Repousuario { get; }
        void Save();
    }
}
