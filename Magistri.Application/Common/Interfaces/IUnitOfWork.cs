using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
       IStudentRepository Students { get; }
        IClassRepository Classes { get; }
        ISubjectRepository Subjects { get; }
        ILessonRepository Lessons { get; }
        ITimeTableEntryRepository TimeTableEntry { get; }
        ITimeTableDayEntryRepository TimeTableDayEntry { get; }
        void Save();
    }
}
