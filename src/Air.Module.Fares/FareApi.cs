using Air.Module.Fares.Messages;
using Air.Module.Fares.Persistance;

namespace Air.Module.Fares;

public class FareApi
{
    public async Task Handle<TMessage>(TMessage message) where TMessage : MessageBase
    {
        switch (message)
        {
            case CreateFares createFares:
                
                break;

            default:
                throw new NotImplementedException($"No handler implemented for message type: {typeof(TMessage).Name}");
        }

        throw new NotImplementedException();
    }
}
