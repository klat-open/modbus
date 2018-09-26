using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DX.Charts
{
    internal class StepLineDiagramImpl : IDiagram
    {
        private XYDiagram _diagram;
        private double _upperLimit;

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

        public StepLineDiagramImpl(XYDiagram diagram)
        {
            _diagram = diagram;
        }

        public void SetAutoScale(bool autoScale)
        {
            var wholeRange = _diagram.AxisY.WholeRange;
            wholeRange.Auto = autoScale;
            wholeRange.MaxValue = _upperLimit;
            wholeRange.MinValue = 0;

            var visualRange = _diagram.AxisY.VisualRange;
            visualRange.Auto = autoScale;
            visualRange.MaxValue = _upperLimit;
            visualRange.MinValue = 0;
        }

        public void SetUpperLimit(double upperLimit)
        {
            _upperLimit = upperLimit;

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
            _diagram.AxisY.WholeRange.AlwaysShowZeroLevel = true;
        }

        public void Dispose()
        {
            _diagram = null;
        }
    }
}