using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Coursework2_Timetable.DTO;
using ScottPlot;

namespace Coursework2_Timetable.View
{
    /// <summary>
    /// Логика взаимодействия для DiagramDateStageWindow1.xaml
    /// </summary>
    public partial class DiagramDateStageWindow1 : Window
    {
        public DiagramDateStageWindow1(List<StagesProject> stages)
        {
            InitializeComponent();
            DataContext = this;
            //КОЛОНКИ

            var plt = new ScottPlot.Plot(600, 400);
            var y = 0;
            plt.AddText("sample text", 10, y, size: 16, System.Drawing.Color.Blue);
            //Create a collection of Bar objects

            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < stages.Count; i++)
            {
                int value = 0;
                if (stages[i].Idstatuse == 1)
                    value = 1;
                 value = (int)stages[i].Idstatuse * 10;
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = $"{stages[i].Title}",
                    LineWidth = 1,
                };
                bars.Add(bar);
            };
            // Add the BarSeries to the plot
            plt.AddBarSeries(bars);
            plt.SetAxisLimitsY(0, 120);
            WpfPlot1.Plot.AddBarSeries(bars);
            WpfPlot1.Refresh();
        }
    }
}
