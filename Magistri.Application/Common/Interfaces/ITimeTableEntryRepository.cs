using Magistri.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Application.Common.Interfaces
{
    public interface ITimeTableEntryRepository : IRepository<TimetableEntry>
    {
        void Update(TimetableEntry entity);
    }
}
