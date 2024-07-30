using MapApplication.Data;


namespace MapApplication.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<PointDb> Points { get; }
        Task<int> CommitAsync();
    }
}

