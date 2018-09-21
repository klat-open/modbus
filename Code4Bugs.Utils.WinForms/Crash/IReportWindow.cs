using System;

namespace Code4Bugs.Utils.WinForms.Crash
{
    public interface IReportWindow
    {
        void Initialize(Exception exception, string productName, string devMail);
        void Show();
    }
}