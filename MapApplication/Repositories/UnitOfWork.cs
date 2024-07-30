using MapApplication.Data;
using MapApplication.Interfaces;


public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IResponseService _responseService;
    private IGenericRepository<PointDb> _pointRepository;

    public UnitOfWork(AppDbContext context, IResponseService responseService)
    {
        _context = context;
        _responseService = responseService;
    }

    public IGenericRepository<PointDb> Points
    {
        get
        {
            return _pointRepository ??= new GenericRepository<PointDb>(_context, _responseService);
        }
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
