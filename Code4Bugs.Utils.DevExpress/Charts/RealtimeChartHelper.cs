using DevExpress.XtraCharts;
using System;
using System.Drawing;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    public sealed partial class RealtimeChartHelper : IDisposable
    {
        private bool _autoScale;
        private double _upperLimit;
        private bool _limitLinesVisible;
        private IDiagram _diagram;
        private ChartControl _chartControl;
        private string _name;
        private bool _onlyInMemory;

        public SeriesBundle PrimarySeriesBundle { get; }
        public SeriesBundle SecondarySeriesBundle { get; }
        public LimitLine HighLine { get; }
        public LimitLine LowLine { get; }

        private bool IsJustInMemory => _chartControl == null;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (PrimarySeriesBundle != null)
                    PrimarySeriesBundle.Name = value;
                if (SecondarySeriesBundle != null)
                    SecondarySeriesBundle.Name = value;
            }
        }

        public bool OnlyInMemory
        {
            get => _onlyInMemory;
            set
            {
                _onlyInMemory = value;

                if (value)
                {
                    PrimarySeriesBundle?.OnlyInMemory();
                    SecondarySeriesBundle?.OnlyInMemory();
                }
                else
                {
                    if (IsJustInMemory)
                        throw new NotSupportedException("The helper must be initialized by a chart control to enable this mode.");
                    PrimarySeriesBundle?.DrawToChart();
                    SecondarySeriesBundle?.DrawToChart();
                }
            }
        }

        public int LiveRangeSeconds { get; set; } = 60;

        public int RoundDigits { get; set; } = 2;

        public int PaddingBottom { get; set; }

        public bool LiveMode { get; set; } = true;

        public bool AutoScale
        {
            get => _autoScale;
            set
            {
                _diagram.SetAutoScale(value);

                if (_autoScale != value)
                {
                    PrimarySeriesBundle.SetAutoScale(value);
                    SecondarySeriesBundle.SetAutoScale(value);
                }

                _autoScale = value;
            }
        }

        public double UpperLimit
        {
            get => _upperLimit;
            set
            {
                if (!(Math.Abs(_upperLimit - value) > 0.001D)) return;

                _upperLimit = value;

                PrimarySeriesBundle.UpperLimit = value;
                PrimarySeriesBundle.SetAutoScale(_autoScale);
                SecondarySeriesBundle.UpperLimit = value;
                SecondarySeriesBundle.SetAutoScale(_autoScale);

                _diagram.SetUpperLimit(value);
            }
        }

        private CachedItem GetGlobalOldestPoint()
        {
            var primaryOldestPoint = PrimarySeriesBundle.OldestPoint;
            var secondaryOldestPoint = SecondarySeriesBundle.OldestPoint;
            if (primaryOldestPoint == null) return secondaryOldestPoint;
            if (secondaryOldestPoint == null) return primaryOldestPoint;
            return primaryOldestPoint.Time < secondaryOldestPoint.Time
                ? primaryOldestPoint
                : secondaryOldestPoint;
        }

        private CachedItem GetGlobalNewestPoint()
        {
            var primaryNewestPoint = PrimarySeriesBundle.NewestPoint;
            var secondaryNewestPoint = SecondarySeriesBundle.NewestPoint;
            if (primaryNewestPoint == null) return secondaryNewestPoint;
            if (secondaryNewestPoint == null) return primaryNewestPoint;
            return primaryNewestPoint.Time > secondaryNewestPoint.Time
                ? primaryNewestPoint
                : secondaryNewestPoint;
        }

        public bool LimitLinesVisible
        {
            get => _limitLinesVisible;
            set
            {
                _limitLinesVisible = value;
                if (HighLine != null)
                    HighLine.Visible = value;
                if (LowLine != null)
                    LowLine.Visible = value;
            }
        }

        public void OptimizePerformance()
        {
            if (_chartControl == null) return;

            _chartControl.RefreshDataOnRepaint = false;
            _chartControl.RuntimeHitTesting = false;
            _chartControl.CacheToMemory = true;
            PrimarySeriesBundle.SetAntialiasing(false);
            SecondarySeriesBundle.SetAntialiasing(false);
        }

        public void OptimizeMemory()
        {
            if (_chartControl == null) return;

            _chartControl.RefreshDataOnRepaint = false;
            _chartControl.RuntimeHitTesting = false;
            _chartControl.CacheToMemory = false;
            PrimarySeriesBundle.SetAntialiasing(false);
            SecondarySeriesBundle.SetAntialiasing(false);
        }

        public void Clear()
        {
            PrimarySeriesBundle.Clear();
            SecondarySeriesBundle.Clear();
        }

        public void Append(double value, DateTime dateTime)
        {
            Append(PrimarySeriesBundle, value, dateTime);
            if (SecondarySeriesBundle != null)
                SecondarySeriesBundle.IsInterrupt = true;
        }

        public void Append2(double value, DateTime dateTime)
        {
            Append(SecondarySeriesBundle, value, dateTime);
            if (PrimarySeriesBundle != null)
                PrimarySeriesBundle.IsInterrupt = true;
        }

        public void CopyFrom(RealtimeChartHelper fromHelper)
        {
            Clear();
            HighLine?.CopyFrom(fromHelper.HighLine);
            LowLine?.CopyFrom(fromHelper.LowLine);
            PrimarySeriesBundle?.CopyFrom(fromHelper.PrimarySeriesBundle);
            SecondarySeriesBundle?.CopyFrom(fromHelper.SecondarySeriesBundle);
        }

        public void Hide()
        {
            PrimarySeriesBundle?.Hide();
            SecondarySeriesBundle?.Hide();
        }

        public void Show()
        {
            PrimarySeriesBundle?.Show();
            SecondarySeriesBundle?.Show();
        }

        public Writer GetWriter()
        {
            return new Writer(
                _chartControl,
                PrimarySeriesBundle,
                _upperLimit,
                PaddingBottom,
                _diagram,
                _autoScale,
                RoundDigits);
        }

        public Writer GetWriter2()
        {
            return new Writer(
                _chartControl,
                SecondarySeriesBundle,
                _upperLimit,
                PaddingBottom,
                _diagram,
                _autoScale,
                RoundDigits);
        }

        private ISeries GetGlobalOldestSeries()
        {
            ISeries primaryOldestSeries = null;
            ISeries secondaryOldestSeries = null;
            if (PrimarySeriesBundle != null)
                primaryOldestSeries = PrimarySeriesBundle.GetOldestSeries();
            if (SecondarySeriesBundle != null)
                secondaryOldestSeries = SecondarySeriesBundle.GetOldestSeries();
            if (primaryOldestSeries == null || primaryOldestSeries.PointCount == 0) return secondaryOldestSeries;
            if (secondaryOldestSeries == null || secondaryOldestSeries.PointCount == 0) return primaryOldestSeries;

            return primaryOldestSeries.OldestPoint.Time < secondaryOldestSeries.OldestPoint.Time
                ? primaryOldestSeries
                : secondaryOldestSeries;
        }

        private void Append(SeriesBundle seriesBundle, double value, DateTime dateTime)
        {
            var series = seriesBundle.GetUpdatableSeries(GetGlobalNewestPoint());
            Append(series, value, dateTime);

            var globalOldestPoint = GetGlobalOldestPoint();
            if (globalOldestPoint != null)
            {
                PrimarySeriesBundle.RemoveOldestIfNecessary(globalOldestPoint.Time);
                SecondarySeriesBundle.RemoveOldestIfNecessary(globalOldestPoint.Time);
            }
        }

        private void Append(ISeries series, double value, DateTime dateTime)
        {
            if (LiveMode)
            {
                var minDateTime = dateTime.AddSeconds(-LiveRangeSeconds - 1);
                if (series.RemoveOutOfRangePoints(minDateTime) == 0)
                {
                    var oldestSeries = GetGlobalOldestSeries();
                    if (oldestSeries != null && oldestSeries != series)
                    {
                        oldestSeries.RemoveOutOfRangePoints(minDateTime);
                        if (oldestSeries.PointCount == 0)
                        {
                            PrimarySeriesBundle?.RemoveSeries(oldestSeries);
                            SecondarySeriesBundle?.RemoveSeries(oldestSeries);
                        }
                    }
                }
                _diagram.SetAxisXMinMaxValues(minDateTime, dateTime);
            }

            if (value < _diagram.AxisYMinValue - PaddingBottom)
            {
                _diagram.AxisYMinValue = value - PaddingBottom;
            }

            var originValue = Math.Round(value, RoundDigits);
            var displayValue = originValue;
            if (!_autoScale && value > _upperLimit)
                displayValue = _upperLimit;

            series.Append(dateTime, displayValue, originValue);
        }

        private ISeriesView CreateSeriesView(ChartControl fromChartControl)
        {
            if (fromChartControl == null) return new DummySeriesView();

            if (fromChartControl.Diagram is SwiftPlotDiagram)
                return new SwiftPlotSeriesViewImpl();
            if (fromChartControl.Diagram is XYDiagram)
                return new StepLineSeriesViewImpl();

            return null;
        }

        private IDiagram CreateDiagram(ChartControl fromChartControl)
        {
            if (fromChartControl == null) return new DummyDiagramImpl();

            if (fromChartControl.Diagram is SwiftPlotDiagram swiftPlotDiagram)
                return new SwiftPlotDiagramImpl(swiftPlotDiagram);
            if (fromChartControl.Diagram is XYDiagram xyDiagram)
                return new StepLineDiagramImpl(xyDiagram);

            return null;
        }

        public RealtimeChartHelper(
            ChartControl chartControl,
            bool showLimitLines,
            bool supportSecondarySeries,
            int seriesOrder,
            string name)
        {
            PrimarySeriesBundle = new SeriesBundle(chartControl, CreateSeriesView, true, seriesOrder, name);
            if (supportSecondarySeries)
            {
                SecondarySeriesBundle = new SeriesBundle(chartControl, CreateSeriesView, false, seriesOrder, name) { SeriesColor = Color.Red };
            }

            _diagram = CreateDiagram(chartControl);
            _chartControl = chartControl;
            _name = name;

            if (showLimitLines && chartControl != null)
            {
                ConstantLine highLine;
                ConstantLine lowLine;
                var constantLines = _diagram.ConstantLines;
                if (constantLines.Count < 2)
                {
                    highLine = new ConstantLine { Color = Color.Red, Title = { Text = @"High" } };
                    lowLine = new ConstantLine { Color = Color.Green, Title = { Text = @"Low" } };
                    constantLines.Add(highLine);
                    constantLines.Add(lowLine);
                }
                else
                {
                    highLine = constantLines[0];
                    lowLine = constantLines[1];
                }

                HighLine = new LimitLine(highLine);
                LowLine = new LimitLine(lowLine);
            }

            _diagram.ShowStretch();

            OnlyInMemory = chartControl == null;
            AutoScale = true;
        }

        public void Dispose()
        {
            _diagram?.Dispose();
            _diagram = null;
            _chartControl = null;
            PrimarySeriesBundle?.Dispose();
            SecondarySeriesBundle?.Dispose();
        }
    }
}