using DevExpress.XtraCharts;
using System.Drawing;

namespace Code4Bugs.Utils.DX.Charts
{
    internal class SwiftPlotSeriesViewImpl : ISeriesView
    {
        private SwiftPlotSeriesView _seriesView;
        private int _thickness;
        private Color _color;
        private bool _antialiasing;
        private DashStyle _dashStyle;

        public SwiftPlotSeriesViewImpl()
        {
            _seriesView = new SwiftPlotSeriesView();
            _thickness = _seriesView.LineStyle.Thickness;
            _color = _seriesView.Color;
            _antialiasing = _seriesView.Antialiasing;
            _dashStyle = _seriesView.LineStyle.DashStyle;
        }

        public int Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                _seriesView.LineStyle.Thickness = value;
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _seriesView.Color = value;
            }
        }

        public bool Antialiasing
        {
            get => _antialiasing;
            set
            {
                _antialiasing = value;
                _seriesView.Antialiasing = value;
            }
        }

        public DashStyle DashStyle
        {
            get => _dashStyle;
            set
            {
                _dashStyle = value;
                _seriesView.LineStyle.DashStyle = value;
            }
        }

        public SeriesViewBase GetOriginView()
        {
            return _seriesView;
        }

        public void CopySettingFrom(ISeriesView fromView)
        {
            Thickness = fromView.Thickness;
            Color = fromView.Color;
            Antialiasing = fromView.Antialiasing;
            DashStyle = fromView.DashStyle;
        }

        public void Dispose()
        {
            _seriesView?.Dispose();
            _seriesView = null;
        }
    }
}