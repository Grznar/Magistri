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
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {

        private readonly ApplicationDbContext _db;

        public SubjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }




        public void Update(Subject entity)
        {
            _db.Update(entity);
        }
    }
}
