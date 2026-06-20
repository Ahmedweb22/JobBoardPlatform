using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Interfaces.IUnitOfWorks
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();

    }
}
