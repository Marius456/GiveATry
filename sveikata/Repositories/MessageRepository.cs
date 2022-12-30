using Microsoft.EntityFrameworkCore;
using giveatry.Models;
using giveatry.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> Create(Message item)
        {
            await _context.Messages.AddAsync(item);
            return item;
        }

        public Task<List<Message>> GetAll()
        {
            return _context.Messages.ToListAsync();
        }

        public Task<Message> GetById(Guid id)
        {
            return _context.Messages.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public Task<List<Message>> GetByPlan(Guid id)
        {
            return _context.Messages.Where(item => item.PlanId.Equals(id)).ToListAsync();
        }

        public void Update(Message item)
        {
            _context.Messages.Update(item);
        }
        public void Delete(Message item)
        {
            _context.Messages.Remove(item);
        }
    }
}
