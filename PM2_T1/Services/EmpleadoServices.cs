using Firebase.Database.Query;
using PM2_T1.Firebase;
using PM2_T1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2_T1.Services
{
    public class EmpleadoServices
    {


        public async Task<bool> InsertarEmpleado(Empleado Empleado)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                .Child("Empleado")
                .PostAsync(new Empleado()
                {
                    Nombre = Empleado.Nombre,
                    Apellidos = Empleado.Apellidos,
                    Edad = Empleado.Edad,
                    Direccion = Empleado.Direccion,
                    Puesto = Empleado.Puesto,
                    Foto = Empleado.Foto
                });
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

        public async Task<List<Empleado>> ListarEmpleados()
        {
            try
            {
                var data = (await Conexion.firebase
                            .Child("Empleado")
                            .OnceAsync<Empleado>()).Select(item => new Empleado
                            {
                                Key = item.Key, // This is the ID
                                Nombre = item.Object.Nombre,
                                Apellidos = item.Object.Apellidos,
                                Direccion = item.Object.Direccion,
                                Edad = item.Object.Edad,
                                Puesto = item.Object.Puesto,
                                Foto = item.Object.Foto,
                            }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<bool> DeleteEmpleado(string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase.Child("Empleado").Child(key).DeleteAsync();
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<bool> UpdateEmpleado(Empleado Empleado, string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                              .Child("Empleado")
                              .Child(key)
                              .PutAsync(Empleado);
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

    }
}
