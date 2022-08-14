using PM2_T1.Models;
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
    public partial class AddPage : ContentPage
    {
        public AddPage(string opcion = "Guardar", Empleado empleado = null)
        {
            InitializeComponent();

            if (opcion.Equals("Guardar"))
            {
                BindingContext = new AddViewModels(imagePersona, opcion, empleado);
            }
            else
            {
                BindingContext = new AddViewModels(imagePersona2, opcion, empleado);
            }
        }
    }
}