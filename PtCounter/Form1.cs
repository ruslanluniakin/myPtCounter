using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using PtCounter.model;

namespace PtCounter
{
    public partial class Form1 : Form
    {
        List<CamPtCounter> camPtCounters;

        FilterInfoCollection videodevices;

        HttpDirector httpDirector;

        HttpClient httpClient;

        CustomButton[] buttonsText;
        CustomButton[] buttonsStart;

        public Form1()
        {
            InitializeComponent();

            videodevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            buttonsText = new CustomButton[videodevices.Count];
            buttonsStart = new CustomButton[videodevices.Count];
            camPtCounters = new List<CamPtCounter>();

            for (int i = 0; i < videodevices.Count; i++)
            {
                buttonsText[i] = new CustomButton(videodevices[i].MonikerString, videodevices[i].Name)
                {
                    Location = new System.Drawing.Point(13, 13 + (i * 24)),
                    Name = $"button{i}text",
                    Size = new System.Drawing.Size(230, 23),
                    Text = $"Устройство {i + 1} {videodevices[i].Name}"
                };

                buttonsStart[i] = new CustomButton(videodevices[i].MonikerString, videodevices[i].Name)
                {
                    Location = new System.Drawing.Point(249, 13 + (i * 24)),
                    Name = $"button{i}start",
                    Size = new System.Drawing.Size(123, 23),
                    BackColor = System.Drawing.Color.Red,
                    ForeColor = System.Drawing.Color.White,
                    TabIndex = 0,
                    Text = "Выключено"
                };

                this.Controls.Add(buttonsText[i]);
                this.Controls.Add(buttonsStart[i]);

                camPtCounters.Add(
                    new CamPtCounter(
                        new VideoCaptureDevice(videodevices[i].MonikerString),
                        videodevices[i].Name,
                        new Rectangle(287, 183, 57, 10))); //// область поиска частиц


                buttonsStart[i].Click += buttonOnOffClickAsync;
            }

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44366/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            httpDirector = new HttpDirector(httpClient, camPtCounters);

            httpDirector.Notify += NotifyWriter;
        }

        public void NotifyWriter(string text)
        {
            textBox1.Text += text + "\r\n";
        }


        public async void buttonOnOffClickAsync(object sender, EventArgs args)
        {
            CustomButton customButton = (CustomButton)sender;
            
            bool check = await httpDirector.StartAsync(customButton.Moniker);
            if (check)
            {
                if (customButton.BackColor == Color.Red)
                {
                    customButton.BackColor = Color.Green;
                    customButton.ForeColor = Color.Black;
                }
                else
                {
                    customButton.BackColor = Color.Red;
                    customButton.ForeColor = Color.White;
                }
            }
        }

        private void Form1_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            httpDirector.AllStopAsync();
        }
    }
}
