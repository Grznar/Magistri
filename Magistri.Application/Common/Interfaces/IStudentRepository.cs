﻿using Magistri.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistri.Application.Common.Interfaces
{
    public interface IStudentRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser entity);
    }
}
