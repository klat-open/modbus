using DevExpress.XtraCharts;
using System;

namespace Code4Bugs.Utils.DevExpress.Charts
{
    internal interface IDiagram : IDisposable
    {
        ConstantLineCollection ConstantLines { get; }

        double AxisYMinValue { get; set; }

        void SetAutoScale(bool autoScale);

        void SetUpperLimit(double upperLimit);

        void SetAxisXMinMaxValues(DateTime minTime, DateTime maxTime);

        void ShowStretch();
    }
}