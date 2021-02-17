using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFrontEnd.Classi;
using XamarinFrontEnd.HttpRequest;

namespace XamarinFrontEnd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecommendationPage : ContentPage
    {

        public List<Product> Products { get; set; }

        public RecommendationPage()
        {
            Products = new List<Product>();
            InitializeComponent();
            FillPage();
        }

        public async void FillPage()
        {
            Products = await GetProduct.GetProductsByRecommendations();
            MyCollectionView.ItemsSource = Products;
        }


    }
}