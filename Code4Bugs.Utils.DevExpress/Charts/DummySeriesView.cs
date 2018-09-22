using DevExpress.XtraCharts;
using System.Drawing;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal class DummySeriesView : ISeriesView
    {
        public int Thickness { get; set; }

        public Color Color { get; set; }

        public bool Antialiasing { get; set; }

        public DashStyle DashStyle { get; set; }

        public SeriesViewBase GetOriginView()
        {
            return null;
        }

        public void CopySettingFrom(ISeriesView fromView)
        {
        }

        public void Dispose()
        {
        }
    }
}