using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinFrontEnd.Classi;

namespace XamarinFrontEnd.ViewModels
{
    public class PlotViewModel
    {
        public PlotModel Model { get; set; }

        public PlotViewModel(List<Price> price_list)
        {
            Model = FillPage(price_list);
        }

        /*
        private PlotModel GetModel()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Sample Pie Chart";
            plotModel1.Background = OxyColors.LightGray;

            var pieSeries1 = new PieSeries();
            pieSeries1.StartAngle = 90;
            pieSeries1.FontSize = 18;
            pieSeries1.FontWeight = FontWeights.Bold;
            pieSeries1.TextColor = OxyColors.LightGray;
            pieSeries1.Slices.Add(new PieSlice("A", 12));
            pieSeries1.Slices.Add(new PieSlice("B", 14));
            pieSeries1.Slices.Add(new PieSlice("C", 16));

            plotModel1.Series.Add(pieSeries1);

            return plotModel1;
        }
        */

        /*
        private PlotModel FillPage(List<Price> price_list)
        {
            
            if (price_list.Any())
            {

                var mod = new PlotModel();
                mod.PlotType = PlotType.XY;
                mod.InvalidatePlot(true);
                mod.Title = "Price History";
                mod.ResetAllAxes();

                var Points = new List<DataPoint>
                {
                    new DataPoint(0, 0),
                    new DataPoint(10, 5),
                    new DataPoint(20, 10),
                    new DataPoint(30, 16),
                    new DataPoint(40, 12),
                    new DataPoint(50, 19)
                };

                var ls1 = new LineSeries();
                ls1.ItemsSource = Points;

                mod.Series.Add(ls1);

                LineSeries s1 = new LineSeries()
                {
                    Title = "Series 1",
                    Color = OxyColors.SkyBlue,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 6,
                    MarkerStroke = OxyColors.White,
                    MarkerFill = OxyColors.SkyBlue,
                    MarkerStrokeThickness = 1.5
                };

                return mod;
               
            }
            else
            {
                return null;
            }
        }
        */

        private PlotModel FillPage(List<Price> price_list)
        {

            if (price_list.Any())
            {

                var mod = new PlotModel();
                mod.PlotType = PlotType.XY;
                mod.InvalidatePlot(true);
                mod.Title = "Price History";
                mod.ResetAllAxes();

                var Points = new List<DataPoint> { };
                foreach (Price price in price_list)
                {
                    Points.Add(new DataPoint(price.price_time.ToOADate(), Double.Parse(price.old_price)));
                };

                var ls1 = new LineSeries();
                ls1.ItemsSource = Points;

                mod.Series.Add(ls1);

                LineSeries s1 = new LineSeries()
                {
                    Title = "Series 1",
                    Color = OxyColors.SkyBlue,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 6,
                    MarkerStroke = OxyColors.White,
                    MarkerFill = OxyColors.SkyBlue,
                    MarkerStrokeThickness = 1.5
                };

                return mod;

            }
            else
            {
                return null;
            }
        }
     
    }
}
