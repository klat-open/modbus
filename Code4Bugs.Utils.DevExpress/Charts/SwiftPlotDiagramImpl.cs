using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal class SwiftPlotDiagramImpl : IDiagram
    {
        private SwiftPlotDiagram _diagram;
        private double _upperLimit;
        private bool _autoScale;

        public ConstantLineCollection ConstantLines => _diagram.AxisY.ConstantLines;

        public double AxisYMinValue
        {
            get => (double)_diagram.AxisY.VisualRange.MinValue;
            set
            {
                _diagram.AxisY.VisualRange.MinValue = value;
                _diagram.AxisY.WholeRange.MinValue = value;
            }
        }

        public SwiftPlotDiagramImpl(SwiftPlotDiagram diagram)
        {
            _diagram = diagram;
        }

        public void SetAutoScale(bool autoScale)
        {
            this._autoScale = autoScale;

            var wholeRange = _diagram.AxisY.WholeRange;
            wholeRange.MaxValue = autoScale ? int.MaxValue : _upperLimit;
            wholeRange.MinValue = autoScale ? int.MinValue : 0;
            wholeRange.Auto = autoScale;

            var visualRange = _diagram.AxisY.VisualRange;
            visualRange.MaxValue = autoScale ? int.MaxValue : _upperLimit;
            visualRange.MinValue = autoScale ? int.MinValue : 0;
            visualRange.Auto = autoScale;
        }

        public void SetUpperLimit(double upperLimit)
        {
            _upperLimit = upperLimit;

            if (_autoScale) return;

            var wholeRangeY = _diagram.AxisY.WholeRange;
            wholeRangeY.MaxValue = upperLimit;
            wholeRangeY.MinValue = 0;
            wholeRangeY.Auto = false;

            var visualRangeY = _diagram.AxisY.VisualRange;
            visualRangeY.Auto = false;
            visualRangeY.MaxValue = upperLimit;
            visualRangeY.MinValue = 0;
        }

        public void SetAxisXMinMaxValues(DateTime minTime, DateTime maxTime)
        {
            _diagram.AxisX.VisualRange.SetMinMaxValues(minTime, maxTime);
        }

        public void SetAxisYMinValue(double minValue)
        {
            _diagram.AxisY.WholeRange.MinValue = minValue;
        }

        public void ShowStretch()
        {
            _diagram.AxisX.WholeRange.SideMarginsValue = 0;
            _diagram.AxisX.VisualRange.SideMarginsValue = 0;
            _diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
        }

        public void Dispose()
        {
            _diagram = null;
        }
    }
}