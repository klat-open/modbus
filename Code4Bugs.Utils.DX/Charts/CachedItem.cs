using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DX.Charts
{
    internal class CachedItem
    {
        public DateTime Time { get; set; }

        public double Value { get; set; }

        public double OriginValue { get; set; }

        public CachedItem(DateTime time, double value, double originValue)
        {
            Time = time;
            Value = value;
            OriginValue = originValue;
        }

        public static CachedItem FromSeriesPoint(SeriesPoint point)
        {
            return new CachedItem(point.DateTimeArgument, point.Values[0], (double)point.Tag);
        }

        public SeriesPoint ToSeriesPoint()
        {
            return new SeriesPoint(Time, Value) { Tag = OriginValue };
        }
    }
}