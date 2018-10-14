using Code4Bugs.Utils.Event;

namespace Code4Bugs.Utils.Tests.Event
{
    internal class DummyContainer
    {
        public delegate void MessageDelegate(DummyMessage msg);

        public event MessageDelegate SubscriberExecuted;

        protected void Trigger(DummyMessage msg)
        {
            SubscriberExecuted?.Invoke(msg);
        }

        [Subscribe]
        protected void SubscriberMethod(DummyMessage msg)
        {
            Trigger(msg);
        }
    }

    internal class InheritDummyContainer : DummyContainer
    {
        [Subscribe]
        private void AnotherSubscriberMethod(DummyMessage msg)
        {
            Trigger(msg);
        }
    }
}