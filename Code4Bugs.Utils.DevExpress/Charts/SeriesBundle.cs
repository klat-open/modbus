using DevExpress.Utils;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    public sealed partial class RealtimeChartHelper
    {
        public sealed partial class SeriesBundle : IDisposable
        {
            private static int _freeId;

            private readonly List<ISeries> _seriesList;
            private ChartControl _chartControl;
            private ISeriesView _seriesView;
            private bool _show = true;
            private readonly bool _enableCrosshair;
            private readonly int _seriesOrder;
            private readonly int _id;
            private readonly Func<ChartControl, ISeriesView> _seriesViewFactory;

            internal bool IsInterrupt { get; set; }

            public string Name { get; set; }

            public int Thickness
            {
                get => GetSeriesView().Thickness;
                set => GetSeriesView().Thickness = value;
            }

            internal double UpperLimit { get; set; }

            internal int RoundDigits { get; set; } = 2;

            public Color SeriesColor
            {
                get => GetSeriesView().Color;
                set => GetSeriesView().Color = value;
            }

            public DashStyle DashStyle
            {
                get => GetSeriesView().DashStyle;
                set => GetSeriesView().DashStyle = value;
            }

            internal CachedItem OldestPoint
            {
                get
                {
                    if (_seriesList.Count == 0)
                        return null;
                    return _seriesList[0].PointCount > 0 ? _seriesList[0].OldestPoint : null;
                }
            }

            internal CachedItem NewestPoint
            {
                get
                {
                    if (_seriesList.Count == 0)
                        return null;
                    var series = _seriesList[_seriesList.Count - 1];
                    return series.PointCount > 0 ? series.NewestPoint : null;
                }
            }

            internal SeriesBundle(
                ChartControl chartControl,
                Func<ChartControl, ISeriesView> seriesViewFactory,
                bool enableCrosshair,
                int seriesOrder,
                string name)
            {
                _chartControl = chartControl;
                _enableCrosshair = enableCrosshair;
                _seriesOrder = seriesOrder;
                Name = name;
                _seriesViewFactory = seriesViewFactory;

                _seriesList = new List<ISeries>();
                _id = _freeId++;
            }

            public void Show()
            {
                _show = true;
                _seriesList?.ForEach(series =>
                {
                    series.Visible = true;
                });
            }

            public void Hide()
            {
                _show = false;
                _seriesList?.ForEach(series =>
                {
                    series.Visible = false;
                });
            }

            internal void SetAntialiasing(bool antialiasing)
            {
                GetSeriesView().Antialiasing = antialiasing;
            }

            internal ISeries GetUpdatableSeries(CachedItem globalNewestPoint)
            {
                var series = GetNewestSeries();

                if (series == null)
                {
                    IsInterrupt = false;
                    return AddSeries(globalNewestPoint);
                }

                if (!IsInterrupt) return series;

                IsInterrupt = false;
                return AddSeries(globalNewestPoint);
            }

            private ISeries AddSeries(CachedItem globalNewestPoint)
            {
                ISeries series;
                if (_isInMemory)
                    series = new SeriesInMemory();
                else
                    series = new SeriesInChart(CreateChartSeries());

                series.Visible = _show;

                RegisterSeries(series);
                if (globalNewestPoint != null)
                    series.Append(globalNewestPoint.Time, globalNewestPoint.Value, globalNewestPoint.OriginValue);

                return series;
            }

            internal void RemoveOldestIfNecessary(DateTime globalOldestTime)
            {
                var oldestSeries = GetOldestSeries();
                if (oldestSeries == null || oldestSeries.Disposed) return;
                var rightValue = oldestSeries.NewestPoint.Time;
                if (rightValue >= globalOldestTime) return;
                RemoveSeries(oldestSeries);
            }

            internal void SetAutoScale(bool autoScale)
            {
                if (autoScale)
                    ExpandChart();
                else
                    CollapseChart();
            }

            internal void Clear()
            {
                foreach (var series in _seriesList)
                {
                    if (series is SeriesInChart seriesChart)
                        _chartControl.Series.Remove(seriesChart.OriginSeries);
                    series.Dispose();
                }
                _seriesList.Clear();

                RefreshSeriesView();
            }

            private void RefreshSeriesView()
            {
                if (_seriesView == null) return;
                var newSeriesView = _seriesViewFactory(_chartControl);
                newSeriesView.CopySettingFrom(_seriesView);
                _seriesView.Dispose();
                _seriesView = newSeriesView;
            }

            private ISeriesView GetSeriesView()
            {
                return _seriesView ?? (_seriesView = _seriesViewFactory(_chartControl));
            }

            private void CollapseChart()
            {
                foreach (var series in _seriesList)
                {
                    series.CollapseSeries(UpperLimit);
                }
            }

            private void ExpandChart()
            {
                foreach (var series in _seriesList)
                {
                    series.ExpandSeries();
                }
            }

            internal ISeries GetOldestSeries()
            {
                return _seriesList.Count == 0 ? null : _seriesList[0];
            }

            internal bool RemoveSeries(ISeries series)
            {
                if (!_seriesList.Remove(series)) return false;

                if (_chartControl != null && series is SeriesInChart seriesInChart)
                {
                    _chartControl.Series.Remove(seriesInChart.OriginSeries);
                    if (_chartControl.Series.Count == 0)
                    {
                        RefreshSeriesView();
                    }
                }

                series.Dispose();
                return true;
            }

            internal int ComputeTotalSeries()
            {
                return _seriesList.Count;
            }

            internal int ComputeTotalPoints()
            {
                return _seriesList.Sum(series => series.PointCount);
            }

            private ISeries GetNewestSeries()
            {
                return _seriesList.Count == 0 ? null : _seriesList[_seriesList.Count - 1];
            }

            private Series CreateChartSeries()
            {
                var series = new Series(Name, ViewType.SwiftPlot)
                {
                    View = GetSeriesView().GetOriginView(),
                    CrosshairEnabled = _enableCrosshair ? DefaultBoolean.True : DefaultBoolean.False,
                    ArgumentScaleType = ScaleType.DateTime,
                    ValueScaleType = ScaleType.Numerical,
                    Tag = _seriesOrder
                };
                return series;
            }

            private void RegisterSeries(ISeries series)
            {
                _seriesList.Add(series);
                if (series is SeriesInChart seriesInChart)
                    CaculateSeriesOrder(seriesInChart);
            }

            private void CaculateSeriesOrder(SeriesInChart seriesInChart)
            {
                var foundIndex = 0;
                var positionForOrder = _seriesOrder;
                for (; foundIndex < _chartControl.Series.Count; foundIndex++)
                {
                    var controlSeries = _chartControl.Series[foundIndex];
                    if (controlSeries.Tag is int order)
                    {
                        if (order >= positionForOrder)
                            break;
                    }
                }

                _chartControl.Series.Insert(foundIndex, seriesInChart.OriginSeries);
            }

            public void CopyFrom(SeriesBundle fromSeriesBundle)
            {
                if (fromSeriesBundle == null) return;

                _chartControl?.BeginInit();
                Clear();
                foreach (var series in fromSeriesBundle._seriesList)
                {
                    var cloneSeries = _isInMemory ? (ISeries)new SeriesInMemory() : new SeriesInChart(CreateChartSeries());
                    cloneSeries.Visible = _show;
                    cloneSeries.CopyFrom(series);
                    RegisterSeries(cloneSeries);
                }
                _chartControl?.EndInit();
            }

            public void SimpleCopySeries(Series toSeries)
            {
                var toPoints = toSeries.Points;
                toPoints.Clear();
                foreach (var series in _seriesList)
                {
                    var fromPoints = series.Points;
                    foreach (var cachedItem in fromPoints)
                    {
                        toPoints.Add(cachedItem.ToSeriesPoint());
                    }
                }
            }

            public void Dispose()
            {
                Clear();

                _seriesView?.Dispose();
                _seriesView = null;

                _chartControl = null;
            }

            public override string ToString()
            {
                return $"{Name}#{_id}";
            }
        }
    }
}