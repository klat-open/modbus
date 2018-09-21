using System.Drawing;

namespace Code4Bugs.Utils.DevExpress.Notifications
{
    public class AlertMessage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public object Content { get; set; }
        public IconType IconType { get; set; }
        public Image CustomIcon { get; set; }
    }

    public enum IconType
    {
        Info,
        Warning,
        Error,
        Success,
        Question,
        Custom
    }
}