using PM2E2Grupo1.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PM2E2Grupo1.Views
{
    public partial class UbicacionesPage : ContentPage
    {
        public List<Location> ubicaciones { get; set; }
        private string latitudSeleccionada;
        private string longitudSeleccionada;

        public UbicacionesPage()
        {
            InitializeComponent();
            PopulateList();
        }

        private async void PopulateList()
        {
            try
            {
                // Create an instance of the LugaresController
                LugaresController lugaresController = new LugaresController();

                // Call the GetLugaresAsync method to fetch data from the database
                List<Lugar> lugares = await lugaresController.GetLugaresAsync();

                // Update the ubicaciones list with the data retrieved from the database
                ubicaciones = new List<Location>();
                foreach (var lugar in lugares)
                {
                    ubicaciones.Add(new Location
                    {
                        titulo = lugar.Descripcion,
                        latitud = lugar.Latitud,
                        longitud = lugar.Longitud
                    });
                }

                ubicacionesListView.ItemsSource = ubicaciones;

                ubicacionesListView.ItemSelected += (sender, e) =>
                {
                    if (e.SelectedItem == null)
                        return;
                    var location = (Location)e.SelectedItem;
                    latitudSeleccionada = location.latitud;
                    longitudSeleccionada = location.longitud;
                };
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database fetch
                await DisplayAlert("Error", $"Error fetching data: {ex.Message}", "OK");
            }
        }

        public async void OnMapaClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(latitudSeleccionada) && !string.IsNullOrEmpty(longitudSeleccionada))
            {
                double latitud = double.Parse(latitudSeleccionada);
                double longitud = double.Parse(longitudSeleccionada);

                await Navigation.PushAsync(new UbicacionMapa(latitud, longitud));
            }
            else
            {
                await DisplayAlert("Error", "No se ha seleccionado una ubicación.", "OK");
            }
        }

        public class Location
        {
            public string titulo { get; set; }
            public string latitud { get; set; }
            public string longitud { get; set; }
            public string display => $"Lat: {latitud}, Lon: {longitud}";
        }
    }
}
