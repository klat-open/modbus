using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    public sealed class Writer
    {
        private readonly ChartControl _chartControl;
        private readonly IDiagram _diagram;
        private readonly RealtimeChartHelper.SeriesBundle _seriesBundle;
        private readonly double _upperLimit;
        private readonly double _paddingBottom;
        private readonly bool _autoScale;
        private readonly int _roundDigits;
        private ISeries _series;
        private double _minValue = double.MaxValue;

        internal Writer(
            ChartControl chartControl,
            RealtimeChartHelper.SeriesBundle seriesBundle,
            double upperLimit,
            double paddingBottom,
            IDiagram diagram,
            bool autoScale,
            int roundDigits)
        {
            _chartControl = chartControl;
            _seriesBundle = seriesBundle;
            _upperLimit = upperLimit;
            _paddingBottom = paddingBottom;
            _diagram = diagram;
            _autoScale = autoScale;
            _roundDigits = roundDigits;
        }

        public void BeginWrite()
        {
            _chartControl.BeginInit();
            _series = _seriesBundle.GetUpdatableSeries(null);
        }

        public void Append(double value, DateTime datetime)
        {
            value = Math.Round(value, _roundDigits);
            var originValue = value;

            if (!_autoScale && originValue > _upperLimit)
            {
                value = _upperLimit;
            }

            if (originValue < _minValue - _paddingBottom)
            {
                _minValue = originValue;
            }

            _series.Append(datetime, value, originValue);
        }

        public void EndWrite()
        {
            _diagram.AxisYMinValue = _minValue - _paddingBottom;
            _chartControl.EndInit();
            _series = null;
        }
    }
}