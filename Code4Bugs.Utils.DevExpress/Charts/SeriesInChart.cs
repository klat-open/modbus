using DevExpress.XtraCharts;
using DevExpress.XtraPrinting.Native;
using System;
using System.Collections.Generic;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal class SeriesInChart : ISeries
    {
        private Series _series;

        public Series OriginSeries => _series;

        public SeriesInChart(Series series)
        {
            _series = series;
        }

        public int PointCount => _series.Points.Count;

        public CachedItem OldestPoint
        {
            get
            {
                var point = _series.Points[0];
                return CachedItem.FromSeriesPoint(point);
            }
        }

        public CachedItem NewestPoint
        {
            get
            {
                var point = _series.Points[_series.Points.Count - 1];
                return CachedItem.FromSeriesPoint(point);
            }
        }

        public IEnumerable<CachedItem> Points
        {
            get
            {
                var points = _series.Points;
                return points
                    .ToArray()
                    .ConvertAll(CachedItem.FromSeriesPoint);
            }
        }

        public bool Disposed { get; private set; }
        public bool Visible { get => _series.Visible; set => _series.Visible = value; }

        public void Append(DateTime time, double value, double originValue)
        {
            var point = new SeriesPoint(time, value) { Tag = originValue };
            _series.Points.Add(point);
        }

        private int ComputePointsToRemoveCount(DateTime minTime)
        {
            var pointsToRemoveCount = 0;
            foreach (SeriesPoint point in _series.Points)
            {
                if (point.DateTimeArgument < minTime)
                    ++pointsToRemoveCount;
                else
                    break;
            }
            return pointsToRemoveCount;
        }

        public int RemoveOutOfRangePoints(DateTime minTime)
        {
            var pointsToRemoveCount = ComputePointsToRemoveCount(minTime);
            if (pointsToRemoveCount <= 0) return 0;
            _series.Points.RemoveRange(0, pointsToRemoveCount);
            return pointsToRemoveCount;
        }

        public void CollapseSeries(double upperLimit)
        {
            foreach (SeriesPoint point in _series.Points)
            {
                var originValue = (double)point.Tag;
                point.Values[0] = Math.Min(originValue, upperLimit);
            }
        }

        public void ExpandSeries()
        {
            foreach (SeriesPoint point in _series.Points)
            {
                var originValue = (double)point.Tag;
                point.Values[0] = originValue;
            }
        }

        private void Clear()
        {
            var points = _series.Points;
            foreach (SeriesPoint point in points)
            {
                point.Dispose();
            }
            points.Clear();
        }

        public void CopyFrom(ISeries series)
        {
            Clear();
            var fromPonits = series.Points;
            var toPoints = _series.Points;
            foreach (var point in fromPonits)
            {
                toPoints.Add(point.ToSeriesPoint());
            }
        }

        public void Dispose()
        {
            if (_series == null || Disposed) return;
            Clear();
            _series.Dispose();
            _series = null;
            Disposed = true;
        }
    }
}