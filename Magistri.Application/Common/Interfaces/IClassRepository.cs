using Magistri.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Application.Common.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        void Update(Class entity);
    }
}
