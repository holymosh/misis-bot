using Domain.Models;

namespace Domain
{
    public interface IVkCallbackApiHandler
    {
        string Handle(CallbackRequest callbackRequest);
    }
}