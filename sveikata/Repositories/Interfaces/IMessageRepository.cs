using giveatry.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAll();

        Task<Message> GetById(Guid id);

        Task<List<Message>> GetByPlan(Guid id);

        Task<Message> Create(Message item);

        void Update(Message item);
        void Delete(Message item);
    }
}
