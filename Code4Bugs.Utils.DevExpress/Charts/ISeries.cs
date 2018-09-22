using System;
using System.Collections.Generic;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal interface ISeries : IDisposable
    {
        int PointCount { get; }
        CachedItem OldestPoint { get; }
        CachedItem NewestPoint { get; }
        IEnumerable<CachedItem> Points { get; }
        bool Disposed { get; }
        bool Visible { get; set; }

        void Append(DateTime time, double value, double originValue);

        int RemoveOutOfRangePoints(DateTime minTime);

        void CollapseSeries(double upperLimit);

        void ExpandSeries();

        void CopyFrom(ISeries series);
    }
}