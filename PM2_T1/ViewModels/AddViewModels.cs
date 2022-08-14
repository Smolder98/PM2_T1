using Plugin.Media;
using Plugin.Media.Abstractions;
using PM2_T1.Models;
using PM2_T1.Services;
using PM2_T1.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PM2_T1.ViewModels
{
    public class AddViewModels : BaseViewModels
    {


        #region VARIABLES

        private string _Nombre;
        private string _Apellidos;
        private string _Edad;
        private string _Direccion;
        private string _Puesto;
        private string _Foto;
        Image imagenEmpleado;
        EmpleadoServices services;
        private string opcion;
        private string key;
        private bool _IsImageDefault;
        private bool _IsImageEdit;
        #endregion

        #region OBJETOS
        public string Nombre
        {
            get { return _Nombre; }
            set
            {
                _Nombre = value;
                OnPropertyChanged();
            }
        }

        public string Apellidos
        {
            get { return _Apellidos; }
            set
            {
                _Apellidos = value;
                OnPropertyChanged();
            }
        }

        public string Edad
        {
            get { return _Edad; }
            set
            {
                _Edad = value;
                OnPropertyChanged();
            }
        }

        public string Direccion
        {
            get { return _Direccion; }
            set
            {
                _Direccion = value;
                OnPropertyChanged();
            }
        }

        public string Puesto
        {
            get { return _Puesto; }
            set
            {
                _Puesto = value;
                OnPropertyChanged();
            }
        }

        public string Foto
        {
            get { return _Foto; }
            set
            {
                _Foto = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageDefault
        {
            get { return _IsImageDefault; }
            set
            {
                _IsImageDefault = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageEdit
        {
            get { return _IsImageEdit; }
            set
            {
                _IsImageEdit = value;
                OnPropertyChanged();
            }
        }


        #endregion


        public AddViewModels(Image imageParam, string opcionReceived, Empleado empleadoReceived)
        {
            imagenEmpleado = imageParam;
            services = new EmpleadoServices();
            opcion = opcionReceived;

            if (opcion.Equals("Editar"))
            {
                CargarParaEditar(empleadoReceived);
                IsImageDefault = false;
                IsImageEdit = true;
            }
            else
            {
                IsImageDefault = true;
                IsImageEdit = false;
            }

            TomarFotoCommand = new Command(async () => await TomarFoto());
            GuardarCommand = new Command(() => GuardarEmpleado());
            ListarCommand = new Command(() => ListarPersonas());
        }

        private void CargarParaEditar(Empleado empleadoReceived)
        {
            Nombre = empleadoReceived.Nombre;
            Apellidos = empleadoReceived.Apellidos;
            Edad = empleadoReceived.Edad;
            Direccion = empleadoReceived.Direccion;
            Puesto = empleadoReceived.Puesto;
            Foto = empleadoReceived.Foto;
            key = empleadoReceived.Key;
        }

        private async void GuardarEmpleado()
        {
            string response = ValidarCampos();
            if (!response.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", response, "Ok");
                return;
            }

            Empleado empleado = new Empleado()
            {
                Nombre = Nombre,
                Apellidos = Apellidos,
                Edad = Edad,
                Direccion = Direccion,
                Puesto = Puesto,
                Foto = Foto
            };

            if (opcion.Equals("Editar"))
            {
                // EDITAR
                bool confirm = await services.UpdateEmpleado(empleado, key);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Actualizado correctamente.", "Ok");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al actualizar.", "Ok");
                }
            }
            else
            {
                // GUARDAR
                bool confirm = await services.InsertarEmpleado(empleado);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Registrado correctamente.", "Ok");
                    Limpiar();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al registrar.", "Ok");
                }
            }

        }

        private void Limpiar()
        {
            Nombre = "";
            Apellidos = "";
            Edad = "";
            Direccion = "";
            Puesto = "";
            Foto = "";
            imagenEmpleado.Source = null;
        }

        private string ValidarCampos()
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                return "Debe existir un nombre.";
            }
            else if (!ValidateOnlyString(Nombre))
            {
                return "Solo debe ingresar letras en su nombres.";
            }
            else if (string.IsNullOrEmpty(Apellidos))
            {
                return "Debe ingresar sus apellidos";
            }
            else if (!ValidateOnlyString(Apellidos))
            {
                return "Solo debe ingresar letras en tu apellidos.";
            }
            else if (string.IsNullOrEmpty(Edad))
            {
                return "Debe ingresar una edad";
            }
            else if (!ValidateOnlyNumber(Edad))
            {
                return "Solo debe ingresar numeros en tu edad";
            }
            else if (string.IsNullOrEmpty(Direccion))
            {
                return "Debe ingresar la direccion";
            }
            else if (string.IsNullOrEmpty(Puesto))
            {
                return "Debe ingresar el puesto";
            }
            else if (string.IsNullOrEmpty(Foto))
            {
                return "Debe ingresar la fotografia";
            }

            return "OK";
        }

        public static bool ValidateOnlyString(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-ZáéíóúÁÉÍÓÚ ]+$");
        }

        public static bool ValidateOnlyNumber(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]*$");
        }

        private async void ListarPersonas()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ListPage());
        }

        private Task TomarFoto()
        {
            GetImageFromCamera();
            return Task.CompletedTask;
        }

        private async void GetImageFromCamera()
        {
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)
                    return;

                imagenEmpleado.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                byte[] byteArray = File.ReadAllBytes(file.Path);
                Foto = System.Convert.ToBase64String(byteArray);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Aviso", "Se produjo un error al tomar la fotografia.", "Ok");
            }
        }

        public ICommand TomarFotoCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand ListarCommand { get; set; }


    }
}
