using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;


namespace PRACTICE
{
   
    public partial class Main : Form
    {
        Containers containers;
        Cities cities;
        WeatherResponce weatherResponce;
        string lat;
        string lon;
      
        public Main()
        {            
            InitializeComponent();
            ReadFile(Setting.path);
            FillSelectBox();
            СontainerAssembly();
            selectbox.SelectedIndex = 0;
           
            
        }

        private void ReadFile(string path)
        {
            cities = JsonConvert.DeserializeObject<Cities>(File.ReadAllText(path));
        }

        private void FillSelectBox()
        {
            foreach (var city in cities.сitylist)
            {
                selectbox.Items.Add(city.name);
            }
            
        }

        private void СontainerAssembly()
        {
            containers = new Containers(new List<Label> { time0, time1, time2, time3, time4, time5, time6, time7 },
                new List<PictureBox> { icon0, icon1, icon2, icon3, icon4, icon5, icon6, icon7 },
                new List<Label> { temp0, temp1, temp2, temp3, temp4, temp5, temp6, temp7 });
        }

        private void WeatherRequest()
        {
            
            string url= "https://api.openweathermap.org/data/2.5/onecall?lat="+lat+"&lon="+lon+"&exclude=daily,minutely,current&units=metric&lang=en&appid="+Setting.apiKey; 
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }
            weatherResponce = JsonConvert.DeserializeObject<WeatherResponce>(response);
            PassForm();
        }

        private void PassForm()
        {

            TimeLabel.Text = "Now " + DateTime.Now.ToShortTimeString();
            TempLabel.Text = Math.Round(weatherResponce.Hourly[0].Temp) + "°";
            WeatherLabel.Text = weatherResponce.Hourly[0].Weather[0].Description;
            TempPic.Load(iconRequst(0));
            FeelLabel.Text = "Feels like " + Math.Round(weatherResponce.Hourly[0].FeelsLike) + "°";
            pressureLabel.Text = Math.Round((weatherResponce.Hourly[0].Pressure / 1.33)).ToString() + "мм рт. ст.";
            HumidityLabel.Text = weatherResponce.Hourly[0].Humidity.ToString() + "%";
            WindLabel.Text = Math.Round(weatherResponce.Hourly[0].WindSpeed, 1).ToString() + "м/c";


            DateTime pDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(weatherResponce.Hourly[0].Dt).ToLocalTime();
            for (int i = 0; i <= 7; i++)
            {
               
                containers.temps[i].Text = Math.Round(weatherResponce.Hourly[i].Temp) + "°";
                containers.icons[i].Load(iconRequst(i));
            }

            time0.Text = pDate.ToShortTimeString();
            for (int i = 1; i <= 7; i++)
            {
                containers.times[i].Text = pDate.AddHours(1).ToShortTimeString();
            }


        }
    

        public string iconRequst(int b)
        {
            string a = "http://openweathermap.org/img/wn/" + weatherResponce.Hourly[b].Weather[0].Icon + "@2x.png";
            return a;
        }

        private void selectbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = selectbox.SelectedIndex;
            City city = cities.сitylist[id];
            lat = city.lat;
            lon = city.lon;
            SityLabel.Text = selectbox.SelectedItem.ToString();
            WeatherRequest();


        }
    }
    class Containers
    {
        public List<Label> times { get; set; }
        public List<PictureBox> icons { get; set; }
        public List<Label> temps { get; set; }
        public Containers(List<Label> times, List<PictureBox> icons, List<Label> temps)
        {
            this.times = times;
            this.icons = icons;
            this.temps = temps;
        }
    }
        



}
