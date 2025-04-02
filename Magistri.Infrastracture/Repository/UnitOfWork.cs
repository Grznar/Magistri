using Magistri.Application.Common.Interfaces;
using Magistri.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Infrastracture.Repository
{
    public  class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IStudentRepository Students { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Students = new StudentRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
