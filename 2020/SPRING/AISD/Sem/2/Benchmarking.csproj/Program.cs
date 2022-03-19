using System;
using System.Windows.Forms;

namespace Benchmarking
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
			//Строку можно раскомментировать для того, что перегенерировать входные данные.
            //BoyerMooreBenchmark.GenerateData();
            var t = BoyerMooreBenchmark.ChartData(new IterationsBenchmark(), 100);
            var data = t.Item1;
            var iterData = t.Item2;
            var form = CreateChartForm(data, iterData);
            Application.Run(form);
        }

        private static Form CreateChartForm(ChartData results, ChartData iterResults)
        {
            var form = new Form {WindowState = FormWindowState.Maximized};
            var chartBuilder = new ChartBuilder();
            var chart = chartBuilder.CreateTimeOfObjectSizeChart(results, "Алгоритм Бойера — Мура");
            var iterChart = chartBuilder.CreateTimeOfObjectSizeChart(iterResults, "Алгоритм Бойера — Мура");
            chart.Dock = DockStyle.Top;
            iterChart.Dock = DockStyle.Bottom;
            form.Controls.Add(chart);
            form.Controls.Add(iterChart);
            form.Resize += (sender, args) => ResizeCharts(form, chart, iterChart);
            form.Shown += (sender, args) => ResizeCharts(form, chart, iterChart);
            return form;
        }

        private static void ResizeCharts(Form form, Control chart, Control iterChart)
        {
            chart.Height = form.ClientSize.Height / 2;
            iterChart.Height = form.ClientSize.Height / 2;
        }
    }
}