using BookStore.Messaging;
using System.Threading.Tasks;

namespace BookStore.Infra.Messaging
{
    public interface IMessageService
    {
        Task SendEmailAsync(
            MessageOptions message,
            params Attachment[] attachments
            );
    }
}
