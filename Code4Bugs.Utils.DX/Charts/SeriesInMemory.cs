using System;
using System.Collections.Generic;
using System.Linq;

namespace Code4Bugs.Utils.DX.Charts
{
    internal class SeriesInMemory : ISeries
    {
        private LinkedList<CachedItem> _cachedItems;

        public int PointCount => _cachedItems.Count;
        public CachedItem OldestPoint => _cachedItems.First.Value;
        public CachedItem NewestPoint => _cachedItems.Last.Value;
        public IEnumerable<CachedItem> Points => _cachedItems.AsEnumerable();
        public bool Disposed { get; private set; }
        public bool Visible { get; set; }

        public SeriesInMemory()
        {
            _cachedItems = new LinkedList<CachedItem>();
        }

        public void Append(DateTime time, double value, double originValue)
        {
            var cachedItem = new CachedItem(time, value, originValue);
            _cachedItems.AddLast(cachedItem);
        }

        private int ComputePointsToRemoveCount(DateTime minTime)
        {
            var pointsToRemoveCount = 0;
            foreach (var item in _cachedItems)
            {
                if (item.Time < minTime)
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
            for (var i = 0; i < pointsToRemoveCount; i++)
            {
                _cachedItems.RemoveFirst();
            }
            return pointsToRemoveCount;
        }

        public void CollapseSeries(double upperLimit)
        {
            foreach (var point in _cachedItems)
            {
                point.Value = Math.Min(point.OriginValue, upperLimit);
            }
        }

        public void ExpandSeries()
        {
            foreach (var point in _cachedItems)
            {
                point.Value = point.OriginValue;
            }
        }

        public void CopyFrom(ISeries series)
        {
            _cachedItems.Clear();
            var fromPoints = series.Points;
            foreach (var point in fromPoints)
            {
                _cachedItems.AddLast(point);
            }
        }

        public void Dispose()
        {
            if (Disposed) return;
            _cachedItems?.Clear();
            _cachedItems = null;
            Disposed = true;
        }
    }
}