using EmployeeManagement.Contexts;
using EmployeeManagement.DataAccess.Contracts;
using EmployeeManagement.DataAccess.Repositories;

namespace EmployeeManagement.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
