using DevExpress.Charts.Native;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Code4Bugs.Utils.DX.Charts
{
    public sealed class YAxisHelper : IDisposable
    {
        private ChartControl _chartControl;
        private string _crosshairFormat;
        private bool _isSynchronizing;
        private IList<ChartControl> _synchronizedChartList;
        private bool _registeredCrosshairFormatHandler;
        private Func<double, string> _crosshairFormatCallback;

        public YAxisHelper(ChartControl chartControl)
        {
            this._chartControl = chartControl;
            SetCrosshairDateTimeFormat("HH:mm:ss");
        }

        public YAxisHelper SetCrosshairFormat(string format)
        {
            _crosshairFormat = format;

            if (_registeredCrosshairFormatHandler) return this;
            _registeredCrosshairFormatHandler = true;

            if (string.IsNullOrEmpty(format) && _crosshairFormatCallback == null)
                _chartControl.CustomDrawCrosshair -= ChartControl_CustomDrawCrosshair;
            else
                _chartControl.CustomDrawCrosshair += ChartControl_CustomDrawCrosshair;
            return this;
        }

        public YAxisHelper SetCrosshairFormatCallback(Func<double, string> callback)
        {
            _crosshairFormatCallback = callback;

            if (_registeredCrosshairFormatHandler) return this;
            _registeredCrosshairFormatHandler = true;

            if (callback == null && string.IsNullOrEmpty(_crosshairFormat))
                _chartControl.CustomDrawCrosshair -= ChartControl_CustomDrawCrosshair;
            else
                _chartControl.CustomDrawCrosshair += ChartControl_CustomDrawCrosshair;
            return this;
        }

        public YAxisHelper SetCrosshairDateTimeFormat(string format)
        {
            _chartControl.CrosshairOptions.GroupHeaderPattern = "{A:" + format + "}";
            return this;
        }

        private void ChartControl_CustomDrawCrosshair(object sender, CustomDrawCrosshairEventArgs e)
        {
            foreach (var group in e.CrosshairElementGroups)
            {
                foreach (var element in group.CrosshairElements)
                {
                    var currentPoint = element.SeriesPoint;
                    var text = _crosshairFormatCallback != null
                        ? _crosshairFormatCallback(currentPoint.Values[0])
                        : string.Format(_crosshairFormat, currentPoint.Values[0]);
                    element.LabelElement.Text = $@"{element.Series.Name}: {text}";
                }
            }
        }

        public YAxisHelper SynchronizeCrosshair(ChartControl toChartControl)
        {
            if (!_isSynchronizing)
            {
                _isSynchronizing = true;
                _synchronizedChartList = new List<ChartControl>();
                _chartControl.MouseMove += ChartControl_MouseMove;
                _chartControl.MouseLeave += ChartControl_MouseLeave;
            }

            _synchronizedChartList.Add(toChartControl);

            return this;
        }

        private void ChartControl_MouseLeave(object sender, EventArgs e)
        {
            foreach (var chartControl in _synchronizedChartList)
            {
                var synchronizedDiagram = (XYDiagram2D)chartControl.Diagram;
                synchronizedDiagram.ShowCrosshair(Point.Empty);
            }
        }

        private void ChartControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var diagram = (XYDiagram2D)_chartControl.Diagram;
            var coord = diagram.PointToDiagram(e.Location);
            if (coord.IsEmpty || coord.ArgumentScaleType != ScaleType.DateTime) return;

            foreach (var chartControl in _synchronizedChartList)
            {
                var synchronizedDiagram = (XYDiagram2D)chartControl.Diagram;
                synchronizedDiagram.ShowCrosshair(e.Location);
            }
        }

        public YAxisHelper CollapseLabel()
        {
            _chartControl.CustomDrawAxisLabel += ChartControl_CustomDrawAxisLabel;
            return this;
        }

        public YAxisHelper ExpandLabel()
        {
            _chartControl.CustomDrawAxisLabel -= ChartControl_CustomDrawAxisLabel;
            return this;
        }

        private void ChartControl_CustomDrawAxisLabel(object sender, CustomDrawAxisLabelEventArgs e)
        {
            var axis = e.Item;
            if (axis.Axis != ((IXYDiagram)_chartControl.Diagram).AxisY)
                return;

            var value = (int)axis.AxisValueInternal;//Math.Abs(axis.AxisValueInternal);

            if (value >= 100000D)
                axis.Text = $"{value / 1000000D}M";
            else if (value >= 1000D)
                axis.Text = $"{value / 1000D}K";
            else if (value >= 100D)
                axis.Text = $"{value}";
            else if (value >= 10D)
                axis.Text = $" {value}";
            else if (value >= 0)
                axis.Text = $"  {value}";
            else if (value > -10D)
                axis.Text = $" {value}";
            else if (value >= -100D)
                axis.Text = $" {value}";
        }

        public void Dispose()
        {
            if (_chartControl == null) return;

            _chartControl.CustomDrawCrosshair -= ChartControl_CustomDrawCrosshair;
            _chartControl.MouseMove -= ChartControl_MouseMove;
            _chartControl.MouseLeave -= ChartControl_MouseLeave;
            _chartControl.CustomDrawAxisLabel -= ChartControl_CustomDrawAxisLabel;

            _chartControl = null;
            _synchronizedChartList?.Clear();
        }
    }
}