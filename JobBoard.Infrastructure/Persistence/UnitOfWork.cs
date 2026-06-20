using System;
using System.Collections.Generic;
using System.Text;
using JobBoard.Core.Interfaces.IUnitOfWorks;

namespace JobBoard.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error:{ex.Message}");
                return 0;
            }
        }
    }
}
