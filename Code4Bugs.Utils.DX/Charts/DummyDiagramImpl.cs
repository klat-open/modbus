using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DX.Charts
{
    internal class DummyDiagramImpl : IDiagram
    {
        public ConstantLineCollection ConstantLines { get; } = null;

        public double AxisYMinValue { get; set; }

        public void SetAutoScale(bool autoScale)
        {
        }

        public void SetUpperLimit(double upperLimit)
        {
        }

        public void SetAxisXMinMaxValues(DateTime minTime, DateTime maxTime)
        {
        }

        public void ShowStretch()
        {
        }

        public void Dispose()
        {
        }
    }
}