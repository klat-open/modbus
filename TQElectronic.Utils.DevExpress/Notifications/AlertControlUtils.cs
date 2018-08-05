using DevExpress.XtraBars.Alerter;
using System.Collections.Generic;
using System.Drawing;
using TQElectronic.Utils.DevExpress.Properties;

namespace TQElectronic.Utils.DevExpress.Notifications
{
    public static class AlertControlUtils
    {
        public static void ShowOrUpdate(this AlertControl alertControl, AlertMessage message)
        {
            var alertFormList = new List<AlertForm>(alertControl.AlertFormList);
            foreach (var alertForm in alertFormList)
            {
                var alertInfo = alertForm.AlertInfo;
                if (alertInfo != null && alertInfo.Tag is AlertMessage oldMessage)
                {
                    if (oldMessage.Id == message.Id)
                    {
                        UpdateAlert(message, alertInfo);
                        return;
                    }
                }
            }

            ShowNewAlert(alertControl, message);
        }

        private static void UpdateAlert(AlertMessage message, AlertInfo alertInfo)
        {
            alertInfo.Caption = message.Title;
            alertInfo.Text = message.Content?.ToString();
            alertInfo.Image = message.IconType == IconType.Custom
                ? message.CustomIcon
                : GetIcon(message.IconType);
            alertInfo.Tag = message;
        }

        private static Image GetIcon(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Info:
                    return Resources.info_icon;

                case IconType.Warning:
                    return Resources.warning_icon;

                case IconType.Error:
                    return Resources.error_icon;

                case IconType.Success:
                    return Resources.success_icon;

                case IconType.Question:
                    return Resources.question_icon;

                default:
                    return null;
            }
        }

        private static void ShowNewAlert(AlertControl alertControl, AlertMessage message)
        {
            var alertInfo = new AlertInfo("", "");
            UpdateAlert(message, alertInfo);
            alertControl.Show(null, alertInfo);
        }
    }
}