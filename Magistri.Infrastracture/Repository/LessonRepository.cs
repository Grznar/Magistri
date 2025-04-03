using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Infrastracture.Repository
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {

        private readonly ApplicationDbContext _db;

        public LessonRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }




        public void Update(Lesson entity)
        {
            _db.Update(entity);
        }
    }
}
