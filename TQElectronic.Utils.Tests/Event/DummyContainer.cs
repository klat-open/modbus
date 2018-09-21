using TQElectronic.Utils.Event;

namespace TQElectronic.Utils.Tests.Event
{
    internal class DummyContainer
    {
        public delegate void MessageDelegate(DummyMessage msg);

        public event MessageDelegate SubscriberExecuted;

        [Subscribe]
        private void SubscriberMethod(DummyMessage msg)
        {
            SubscriberExecuted?.Invoke(msg);
        }
    }
}