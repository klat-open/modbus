using DevExpress.XtraCharts;
using System.Drawing;

namespace Code4Bugs.Utils.DX.Charts
{
    public sealed partial class RealtimeChartHelper
    {
        public sealed class LimitLine
        {
            private readonly ConstantLine _line;

            public double Value
            {
                get => (double)_line.AxisValue;
                set => _line.AxisValue = value;
            }

            public Color Color
            {
                get => _line.Color;
                set => _line.Color = value;
            }

            public bool Visible
            {
                get => _line.Visible;
                set => _line.Visible = value;
            }

            public LimitLine(ConstantLine line)
            {
                _line = line;
            }

            public void CopyFrom(LimitLine fromLine)
            {
                if (fromLine == null) return;
                Value = fromLine.Value;
                Color = fromLine.Color;
                Visible = fromLine.Visible;
            }
        }
    }
}