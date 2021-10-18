// This code will eventually be generated.

using Microsoft.StandardUI.Wpf;
using System.Collections.Generic;
using Microsoft.StandardUI;

namespace Microcharts.Wpf
{
    public abstract class Chart : StandardControl, IChart
    {
        public static readonly System.Windows.DependencyProperty EntriesProperty = PropertyUtils.Register(nameof(Entries), typeof(IEnumerable<ChartEntry>), typeof(Chart), null);
        public static readonly System.Windows.DependencyProperty BackgroundColorProperty = PropertyUtils.Register(nameof(BackgroundColor), typeof(ColorWpf), typeof(Chart), ColorWpf.Default);
        public static readonly System.Windows.DependencyProperty LabelColorProperty = PropertyUtils.Register(nameof(LabelColor), typeof(ColorWpf), typeof(Chart), ColorWpf.Default);

        public IEnumerable<ChartEntry> Entries
        {
            get => (IEnumerable<ChartEntry>)GetValue(EntriesProperty);
            set => SetValue(EntriesProperty, value);
        }

        public ColorWpf BackgroundColor
        {
            get => (ColorWpf)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        Color IChart.BackgroundColor
        {
            get => BackgroundColor.Color;
            set => BackgroundColor = new ColorWpf(value);
        }

        public ColorWpf LabelColor
        {
            get => (ColorWpf)GetValue(LabelColorProperty);
            set => SetValue(LabelColorProperty, value);
        }
        Color IChart.LabelColor
        {
            get => LabelColor.Color;
            set => LabelColor = new ColorWpf(value);
        }
    }
}
