﻿using Magistri.Application.Common.Interfaces;
using Magistri.Domain.Entities;
using Magistri.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Infrastracture.Repository
{
    public class TimeTableEntryRepository : Repository<TimeTableEntry>, ITimeTableEntryRepository
    {

        private readonly ApplicationDbContext _db;

        public TimeTableEntryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }




        public void Update(TimeTableEntry entity)
        {
            _db.Update(entity);
        }
    }
}
