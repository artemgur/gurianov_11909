using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace Benchmarking
{
    public class ChartBuilder
    {
        public Control CreateTimeOfObjectSizeChart(ChartData chartData, string title)
        {
            var chart = new ZedGraphControl();
            chart.GraphPane.YAxis.Title.Text = chartData.YAxisLabel;
            chart.GraphPane.XAxis.Title.Text = "Size";
            chart.GraphPane.Title.Text = title;
            chart.GraphPane.AddCurve(
                chartData.CurveLabel,
                chartData.Results.Select(z => (double) z.InputSize).ToArray(),
                chartData.Results.Select(z => z.AverageResult).ToArray(),
                Color.Red);
            chart.GraphPane.XAxis.Scale.MinAuto = true;
            chart.GraphPane.XAxis.Scale.MaxAuto = true;
            chart.GraphPane.YAxis.Scale.MinAuto = true;
            chart.GraphPane.YAxis.Scale.MaxAuto = true;
            chart.AxisChange();
            chart.Invalidate();
            return chart;
        }
    }
}