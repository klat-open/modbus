using System.Collections.Generic;

namespace Code4Bugs.Utils.DX.Charts
{
    public sealed partial class RealtimeChartHelper
    {
        public sealed partial class SeriesBundle
        {
            private bool _isInMemory;

            internal void OnlyInMemory()
            {
                if (_isInMemory) return;

                _isInMemory = true;

                var tempSeriesInMemories = new List<ISeries>(_seriesList.Count);
                foreach (var seriesInChart in _seriesList)
                {
                    var seriesInMemory = new SeriesInMemory();
                    seriesInMemory.CopyFrom(seriesInChart);
                    tempSeriesInMemories.Add(seriesInMemory);
                }

                Clear();
                _seriesList.AddRange(tempSeriesInMemories);
            }

            internal void DrawToChart()
            {
                if (!_isInMemory) return;

                _isInMemory = false;

                var tempSeriesCharts = new List<ISeries>(_seriesList.Count);
                foreach (var seriesInMemory in _seriesList)
                {
                    var seriesInChart = new SeriesInChart(CreateChartSeries());
                    seriesInChart.CopyFrom(seriesInMemory);
                    tempSeriesCharts.Add(seriesInChart);
                }

                _chartControl.BeginInit();
                Clear();
                tempSeriesCharts.ForEach(RegisterSeries);
                tempSeriesCharts.Clear();
                _chartControl.EndInit();
            }
        }
    }
}