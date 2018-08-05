using System;

namespace TQElectronic.Utils.Event
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SubscribeAttribute : Attribute
    {
        public ThreadMode ThreadMode { get; set; } = ThreadMode.Post;
    }
}