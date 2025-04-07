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
    public class MessageRepository : Repository<Message>, IMessageRepository
    {

        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }




        public void Update(Message entity)
        {
            _db.Update(entity);
        }
    }
}
