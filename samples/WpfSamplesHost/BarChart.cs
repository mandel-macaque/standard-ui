// This code will eventually be generated.

using Microcharts;

namespace SimpleControls.Wpf
{
    public class BarChart : PointChart, IBarChart
    {
        public BarChart()
        {
            InitImplementation(new BarChartImplementation(this));
        }
    }
}
