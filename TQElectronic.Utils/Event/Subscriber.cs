using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace TQElectronic.Utils.Event
{
    internal class Subscriber
    {
        private readonly SubscribeAttribute _subscribeAttribute;
        private readonly MethodInfo _subscriberMethodInfo;
        private readonly Type _subscribeType;
        private IThreadHelper _threadHelper;

        public void SetThreadHelper(IThreadHelper threadHelper)
        {
            this._threadHelper = threadHelper;
        }

        public Subscriber(SubscribeAttribute subscribeAttribute, MethodInfo subscriberMethodInfo)
        {
            this._subscribeAttribute = subscribeAttribute;
            this._subscriberMethodInfo = subscriberMethodInfo;
            ParameterInfo[] parameters = this._subscriberMethodInfo.GetParameters();
            if (parameters.Length != 1)
                throw new ArgumentException("Subscriber method must have only one parameter.");
            this._subscribeType = parameters[0].ParameterType;
        }

        public void Execute(object container, object message)
        {
            if (!this._subscribeType.IsInstanceOfType(message))
                return;
            switch (this._subscribeAttribute.ThreadMode)
            {
                case ThreadMode.Post:
                    this.ExecuteSubscriber(container, message);
                    break;

                case ThreadMode.Thread:
                    this.ExecuteSubscriberInBackground(container, message);
                    break;

                case ThreadMode.Async:
                    this.ExecuteSubscriberAsync(container, message);
                    break;

                case ThreadMode.Main:
                    this.ExecuteSubscriberInMain(container, message);
                    break;

                case ThreadMode.MainOrder:
                    this.ExecuteSubscriberInMainOrder(container, message);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteSubscriberInMainOrder(object container, object message)
        {
            this._threadHelper.RunInMain((Action)(() => this.ExecuteSubscriber(container, message)), false);
        }

        private void ExecuteSubscriberInMain(object container, object message)
        {
            this._threadHelper.RunInMain((Action)(() => this.ExecuteSubscriber(container, message)), true);
        }

        private void ExecuteSubscriberAsync(object container, object message)
        {
            Task.Run((Action)(() => this.ExecuteSubscriber(container, message)));
        }

        private void ExecuteSubscriberInBackground(object container, object message)
        {
            if (this._threadHelper.IsMainThread())
                this._threadHelper.NewThread((Action)(() => this.ExecuteSubscriber(container, message)));
            else
                this.ExecuteSubscriber(container, message);
        }

        private void ExecuteSubscriber(object container, object message)
        {
            this._subscriberMethodInfo.Invoke(container, BindingFlags.Instance | BindingFlags.InvokeMethod, (Binder)null, new object[1]
            {
        message
            }, CultureInfo.CurrentCulture);
        }
    }
}