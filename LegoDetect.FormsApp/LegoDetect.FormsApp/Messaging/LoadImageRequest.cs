namespace LegoDetect.FormsApp.Messaging;

using System;

using Smart;
using Smart.Forms.Messaging;

public sealed class LoadImageRequest : IEventRequest<EventArgs<byte[]>>
{
    public event EventHandler<EventArgs<byte[]>>? Requested;

    public void Load(byte[] data)
    {
        Requested?.Invoke(this, new EventArgs<byte[]>(data));
    }
}
