using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Controls;

namespace Microcharts
{
    [StandardControl]
    public interface IChart : IStandardControl
    {
        /// <summary>
        /// The type of chart (bar chart, pie chart, etc.)
        /// </summary>
        [DefaultValue(ChartType.BarChart)]
        ChartType ChartType { get; set; }

        /// <summary>
        /// Data for the chart
        /// </summary>
        IEnumerable<ChartEntry> Entries { get; set; }

        /// <summary>
        /// Color used for chart background
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Color used for chart labels
        /// </summary>
        Color LabelColor { get; set; }
    }

    public class Chart : StandardControl<IChart>
    {
        private ChartBase _chart;

        public Chart(IChart control) : base(control)
        {
        }

        /// <summary>
        /// Build the UI element hierarchy for the chart control, for the current chart type.
        /// </summary>
        public override IUIElement Build()
        {
            switch (Control.ChartType)
            {
                case ChartType.BarChart:
                    _chart = new BarChart(Control);
                    break;

                case ChartType.PointChart:
                    _chart = new PointChart(Control);
                    break;

                case ChartType.RadarChart:
                    _chart = new RadarChart(Control);
                    break;
            }

            return _chart.Build();
        }
    }
}
