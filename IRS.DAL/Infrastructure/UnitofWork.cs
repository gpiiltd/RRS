using IRS.DAL.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure
{
    public class UnitofWork : IUnitofWork
    {
        private readonly ApplicationDbContext context;
        public UnitofWork(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
