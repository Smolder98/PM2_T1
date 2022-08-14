using PM2_T1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2_T1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        ListViewModels listaViewModels;
        public ListPage()
        {
            InitializeComponent();
            listaViewModels = new ListViewModels();
            BindingContext = listaViewModels;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listaViewModels.CargarDatos();
        }
    }
}