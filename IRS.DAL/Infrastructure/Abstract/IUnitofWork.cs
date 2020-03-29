using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Infrastructure.Abstract
{    
    public interface IUnitofWork
    {
        Task CompleteAsync();
    }
}
