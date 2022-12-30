using Serilog;
using giveatry.DTOs;
using giveatry.Mappers;
using giveatry.Repositories.Interfaces;
using giveatry.Services.Interfaces;
using giveatry.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace giveatry.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, AppDbContext context)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<MessageResponse> Create(MessageDTO newMessage)
        {
            var message = MessageMapper.Map(newMessage);

            if (message.Text == null)
            {
                string errorMessage2 = "Message not found.";
                Log.Error(errorMessage2);
                return new MessageResponse(errorMessage2);
            }
            message.Time = DateTime.Now;
            try
            {
                await _messageRepository.Create(message);
                await _context.SaveChangesAsync();
                return new MessageResponse(MessageMapper.Map(message));
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new MessageResponse(errorMessage);
            }
        }

        public async Task<IEnumerable<MessageDTO>> GetAll()
        {
            return (await _messageRepository.GetAll()).Select(MessageMapper.Map).ToList();
        }

        public async Task<MessageResponse> GetById(Guid id)
        {
            var message = await _messageRepository.GetById(id);

            if (message == null)
            {
                string errorMessage = "Message not found.";

                Log.Error(errorMessage);

                return new MessageResponse(errorMessage);
            }

            return new MessageResponse(MessageMapper.Map(message));
        }

        public async Task<IEnumerable<MessageDTO>> GetByPlan(Guid id)
        {
            var messages = (await _messageRepository.GetByPlan(id)).Select(MessageMapper.Map).ToList();

            foreach (var item in messages)
            {
                var user = _userRepository.GetById(item.UserId);
                item.SenderName = user.Result.Name;
                item.SenderImage = user.Result.Image;
            }

            return messages;
        }

        public async Task<MessageResponse> Update(Guid id, MessageDTO updatedMessage)
        {
            var message = await _messageRepository.GetById(id);
            if (message == null)
            {
                throw new KeyNotFoundException();
            }

            message.Id = updatedMessage.Id;
            message.Time = updatedMessage.Time;
            message.Text = updatedMessage.Text;

            if (message.Text == null)
            {
                string errorMessage2 = "Message not found.";
                Log.Error(errorMessage2);
                return new MessageResponse(errorMessage2);
            }
            try
            {
                _messageRepository.Update(message);
                await _context.SaveChangesAsync();
                return new MessageResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new MessageResponse(errorMessage);
            }
        }


        public async Task<MessageResponse> Delete(Guid id)
        {
            var item = await _messageRepository.GetById(id);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                _messageRepository.Delete(item);
                await _context.SaveChangesAsync();
                return new MessageResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new MessageResponse(errorMessage);
            }
        }

    }
}
