using OxyPlot;
using OxyPlot.Axes;
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


        private PlotModel FillPage(List<Price> price_list)
        {

            if (price_list.Any())
            {

                var mod = new PlotModel();
                mod.PlotType = PlotType.XY;
                mod.InvalidatePlot(true);
                mod.Title = "Price History";
                mod.ResetAllAxes();

                mod.Axes.Add(new LinearAxis()
                {
                    Position = AxisPosition.Left,
                    Title = "Price"
                });

                mod.Axes.Add(new DateTimeAxis()
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "yyyy-MM-dd HH:mm:ss",
                    Title = "Date"
                });

                //"yyyy-MM-ddTHH:mm:ss"
                //2021-02-08T17:30:14.280
                var Points = new List<DataPoint> { };
                foreach (Price price in price_list)
                {            
                    Points.Add(new DataPoint(price.Price_time.ToOADate(), Double.Parse(price.Old_price)));
                };

                var ls1 = new LineSeries()
                {
                    Title = "Series 1",
                    Color = OxyColors.SkyBlue,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 10,
                    MarkerStroke = OxyColors.White,
                    MarkerFill = OxyColors.SkyBlue,
                    MarkerStrokeThickness = 1.5
                };
                ls1.ItemsSource = Points;
                mod.Series.Add(ls1);

                return mod;

            }
            else
            {
                return null;
            }
        }
     
    }
}
