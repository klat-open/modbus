using DevExpress.XtraCharts;
using System;
using System.Drawing;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal interface ISeriesView : IDisposable
    {
        int Thickness { get; set; }
        Color Color { get; set; }
        bool Antialiasing { get; set; }
        DashStyle DashStyle { get; set; }

        SeriesViewBase GetOriginView();

        void CopySettingFrom(ISeriesView fromView);
    }
}