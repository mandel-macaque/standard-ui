using Microcharts;
using Microsoft.StandardUI;
using Microsoft.StandardUI.Wpf;
using Microsoft.StandardUI.Wpf.NativeVisualEnvironment;
using System.Windows;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            WpfStandardUIEnvironment.Init(new WpfNativeVisualEnvironment());
            InitializeComponent();

            MyBarChart.Entries = CreateChartEntries();
            MyPointChart.Entries = CreateChartEntries();
#if false
            var barChart = new BarChart()
            {
                Entries = CreateChartEntries(),
                BackgroundColor = Colors.Blue,
                LabelColor = Colors.Green,
                Width = 400,
                Height = 300,
            };
            controlStack.Children.Add(barChart);

            var pointChart = new PointChart()
            {
                Entries = CreateChartEntries(),
                BackgroundColor = Colors.Red,
                LabelColor = Colors.Maroon,
                Width = 400,
                Height = 300,
            };
            controlStack.Children.Add(pointChart);
#endif

#if false
            var radarChart = new RadarChart()
            {
                Entries = CreateChartEntries(),
                LabelTextSize = 14,
                IsAnimated = false,
                Width = 400,
                Height = 400,
            };
            radarChart.Build();

            var radarChartWpf = new StandardUIUserControlWpf(radarChart);
            radarChartWpf.HorizontalAlignment = HorizontalAlignment.Left;
            controlStack.Children.Add(radarChartWpf);
#endif
        }

        public static ChartEntry[] CreateChartEntries()
        {
            return new[]
            {
                new ChartEntry(200)
                {
                        Label = "January",
                        ValueLabel = "200",
                        Color = Color.FromHex("#266489")
                },
                new ChartEntry(400)
                {
                        Label = "February",
                        ValueLabel = "400",
                        Color = Color.FromHex("#68B9C0"),
                },
                new ChartEntry(100)
                {
                        Label = "March",
                        ValueLabel = "100",
                        Color = Color.FromHex("#90D585"),
                },
            };
        }
    }
}
