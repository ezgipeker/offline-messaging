using Microsoft.EntityFrameworkCore;
using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Domain.Dtos.Message;
using OfflineMessaging.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Message
{
    public class CrudMessageServices : ICrudMessageServices
    {
        private readonly OfflineMessagingContext _context;
        private readonly ICrudUserServices _crudUserServices;

        public CrudMessageServices(OfflineMessagingContext context, ICrudUserServices crudUserServices)
        {
            _context = context;
            _crudUserServices = crudUserServices;
        }

        public async Task<bool> AddMessageAsync(MessageDto parameters)
        {
            await _context.Messages.AddAsync(new Domain.Entities.Message
            {
                CreateDate = DateTime.Now,
                SenderUserId = parameters.SenderUserId,
                ReceiverUserId = parameters.ReceiverUserId,
                Content = parameters.Content
            });

            var result = await _context.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<MessageHistoryDto> GetLastMessageAsync(GetLastMessageParametersDto parameters)
        {
            var lastMessage = await _context.Messages.OrderByDescending(x => x.CreateDate)
                .FirstOrDefaultAsync(x => x.SenderUserId == parameters.SenderUserId && x.ReceiverUserId == parameters.ReceiverUserId);

            if (lastMessage == null)
            {
                return null;
            }

            var result = new MessageHistoryDto
            {
                Id = lastMessage.Id,
                SenderUserId = lastMessage.SenderUserId,
                SenderUserName = _crudUserServices.GetUser(lastMessage.SenderUserId).UserName,
                ReceiverUserId = lastMessage.ReceiverUserId,
                ReceiverUserName = _crudUserServices.GetUser(lastMessage.ReceiverUserId).UserName,
                SendDate = lastMessage.CreateDate,
                Content = lastMessage.Content,
                IsRead = lastMessage.IsRead,
                ReadDate = lastMessage.ReadDate
            };

            return result;
        }

        public async Task<List<MessageHistoryDto>> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters)
        {
            var messageList = await _context.Messages.Where(x => (x.SenderUserId == parameters.SenderUserId && x.ReceiverUserId == parameters.ReceiverUserId) || (x.SenderUserId == parameters.ReceiverUserId && x.ReceiverUserId == parameters.SenderUserId))
                .OrderByDescending(x => x.CreateDate)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var result = messageList.Select(x => new MessageHistoryDto
            {
                Id = x.Id,
                SenderUserId = x.SenderUserId,
                SenderUserName = _crudUserServices.GetUser(x.SenderUserId).UserName,
                ReceiverUserId = x.ReceiverUserId,
                ReceiverUserName = _crudUserServices.GetUser(x.ReceiverUserId).UserName,
                SendDate = x.CreateDate,
                Content = x.Content,
                IsRead = x.IsRead,
                ReadDate = x.ReadDate
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateMessageReadInfoAsync(int messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);
            message.IsRead = true;
            message.ReadDate = DateTime.Now;

            _context.Messages.Update(message);

            var result = await _context.SaveChangesAsync() > 0;

            return result;
        }
    }
}
