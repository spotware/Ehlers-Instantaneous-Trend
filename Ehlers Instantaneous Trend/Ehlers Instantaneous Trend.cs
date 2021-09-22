using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None), Cloud("Trigger", "Trend")]
    public class EhlersInstantaneousTrend : Indicator
    {
        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Alpha", DefaultValue = 0.07, Step = 0.01)]
        public double Alpha { get; set; }

        [Output("Trigger", LineColor = "Blue", PlotType = PlotType.Line)]
        public IndicatorDataSeries Trigger { get; set; }

        [Output("Trend", LineColor = "Red", PlotType = PlotType.Line)]
        public IndicatorDataSeries Trend { get; set; }

        protected override void Initialize()
        {
        }

        public override void Calculate(int index)
        {
            Trend[index] = (Alpha - ((Alpha * Alpha) / 4.0)) * Source[index] + 0.5 * Alpha * Alpha * Source[index - 1] -
                (Alpha - 0.75 * Alpha * Alpha) * Source[index - 2] + 2 * (1 - Alpha) *
                GetValueOrDefault(Trend, index - 1, ((Source[index] + 2 * Source[index - 1] + Source[index - 2]) / 4.0)) - (1 - Alpha) * (1 - Alpha) *
                GetValueOrDefault(Trend, index - 2, ((Source[index] + 2 * Source[index - 1] + Source[index - 2]) / 4.0));

            Trigger[index] = 2.0 * Trend[index] - Trend[index - 2];
        }

        private double GetValueOrDefault(DataSeries series, int index, double defaultValue = 0)
        {
            var value = series[index];

            return double.IsNaN(value) ? defaultValue : value;
        }
    }
}