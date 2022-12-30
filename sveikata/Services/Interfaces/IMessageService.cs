using giveatry.DTOs;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace giveatry.Services.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetAll();

        Task<MessageResponse> GetById(Guid id);

        Task<IEnumerable<MessageDTO>> GetByPlan(Guid id);

        Task<MessageResponse> Create(MessageDTO item);

        Task<MessageResponse> Update(Guid id, MessageDTO item);

        Task<MessageResponse> Delete(Guid id);
    }
}
