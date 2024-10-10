using Avalonia.Controls;
using System.Net;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Avalonia.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System.IO;
using Microsoft.CodeAnalysis;

namespace blitzWeather
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = mesto;

            seznamMest.DataContext = listaMest;

            seznamProg.DataContext = seznamMest.SelectedItem;

            seznamMest.SelectedIndex = 0;
        }

        public Mesto mesto = MestoAPI("Maribor");
        public static List<Mesto> listaMest = GetMesta();



        public static Mesto MestoAPI(string location)
        {
            Mesto mesto = new Mesto();
            List<Prognoza> prognoze = new List<Prognoza>();
            HttpClient client = new HttpClient();
            string apiKey = /*API KEY*/;

            // Build the URL for the API request
            string url = $"http://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={location}&days=5&aqi=no&alerts=no";

            // Make the API request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string content = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

            // Parse the JSON response
            JObject weatherData = JObject.Parse(content);

            // Extract the current temperature and humidity from the response
            string country = (string)weatherData["location"]["country"];
            string temperature = (string)weatherData["current"]["temp_c"];
            string humidity = "Humidity: " + (string)weatherData["current"]["humidity"] + "%";
            string icon = "http:" + (string)weatherData["current"]["condition"]["icon"];
            string conditionText = (string)weatherData["current"]["condition"]["text"];

            using (HttpClient klijent = new HttpClient())
            using (HttpResponseMessage odgovor = client.GetAsync(icon).Result)
            using (Stream stream = odgovor.Content.ReadAsStreamAsync().Result)
            {
                var bitmap = new Bitmap(stream);
                mesto.Ikonica = bitmap;
            }

            JArray forecast = (JArray)weatherData["forecast"]["forecastday"];

            foreach (JToken day in forecast)
            {
                
                Prognoza prognoza = new Prognoza();
                string datum = (string)day["date"];
                string minTemp = (string)day["day"]["mintemp_c"];
                string maxTemp = (string)day["day"]["maxtemp_c"];
                string slika = "http:" + (string)day["day"]["condition"]["icon"];

                prognoza.Datum = DateTime.Parse(datum);
                prognoza.DanVTednu = prognoza.Datum.ToString("ddd");
                prognoza.DnevnaTemperaturaMin = minTemp + "�C";
                prognoza.DnevnaTemperaturaMax = maxTemp + "�C";

                using (HttpClient klijent = new HttpClient())
                using (HttpResponseMessage odgovor = client.GetAsync(slika).Result)
                using (Stream stream = odgovor.Content.ReadAsStreamAsync().Result)
                {
                    var bitmap = new Bitmap(stream);
                    prognoza.DnevnaSlika = bitmap;
                }

                prognoze.Add(prognoza);
            }

            mesto.Ime = location;
            mesto.Drzava = country;
            mesto.Temperatura = temperature + "�C";
            mesto.VlagaZraka = humidity;
            mesto.Stanje = conditionText;
            mesto.SeznamPrognoz = prognoze;

            return mesto;
        }

        public static List<Mesto> GetMesta()
        {
            List<Mesto> mesta = new List<Mesto>();

            string[] lokacije = { "Maribor", "Ljubljana", "Celje", "Zagreb", "London", "New York", "Ni�", "Beograd" };

            foreach(var item in lokacije)
            {
                Mesto mes = MestoAPI(item);
                mesta.Add(mes);
            }

            return mesta;
        }


    }
}
