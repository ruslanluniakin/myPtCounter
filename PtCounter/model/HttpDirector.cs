using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PtCounter.model
{
    class HttpDirector
    {
        private HttpClient httpClient;

        readonly List<CamPtCounter> camPtCounters;

        public delegate void newAction(string actionString);

        public event newAction Notify;



        public HttpDirector(HttpClient httpClient, List<CamPtCounter> camPtCounters)
        {
            this.httpClient = httpClient;

            this.camPtCounters = camPtCounters;

            this.camPtCounters.ForEach(x => x.sendNewReport += sendReportAsync);
        }

        public async Task<bool> StartAsync(string Moniter)
        {
            var camDevice = camPtCounters.FirstOrDefault(x => x.camSource.Source == Moniter);

            if (camDevice.camSource.IsRunning)
            {
                camDevice.StopCounting();
                Notify?.Invoke("Остановлен " + Moniter);
                return true;
            }
            else
            {
                var check = await createDevice(camDevice);

                if (check)
                {
                    camDevice.StartCounting();
                    Notify?.Invoke("Запущен " + Moniter);
                    return true;
                }
                else
                {
                    Notify?.Invoke("Запуск не удался");
                    return false;
                }
            }
        }

        public void AllStopAsync()
        {
            foreach (var item in camPtCounters)
            {
                if (item.camSource.IsRunning)
                {
                    item.camSource.SignalToStop();
                    Notify?.Invoke("Остановлен " + item.camSource.Source);
                }
            }
        }

        private async Task<bool> createDevice(CamPtCounter camPtCounter)
        {
            try
            {
                HttpResponseMessage response =
                await httpClient.PostAsJsonAsync("api/Device/",
                                                new Device()
                                                {
                                                    Name = camPtCounter.Name,
                                                    Moniker = camPtCounter.camSource.Source
                                                });

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Notify?.Invoke("Девайс добавлен или уже существует " + camPtCounter.camSource.Source);
                    return true;
                }
                else
                {
                    throw new Exception("При добавлении девайса прозошла ошибка. Код - " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
                return false;
            }
        }

        private async Task sendReportAsync(ReportCount reportCount, string Moniker)
        {
            ReportFromClient reportFromClient = new ReportFromClient()
            {
                Count = reportCount.Count,
                Moniker = Moniker,
                dateTime = reportCount.dateTime.Ticks,
                Duration = reportCount.Duration.Ticks
            };

            try
            {
                HttpResponseMessage response =
                    await httpClient.PostAsJsonAsync("api/Report/", reportFromClient);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Notify?.Invoke("Отчет отправлен " + Moniker);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Notify?.Invoke("Такого устройсва не существует" + Moniker);
                }
            }
            catch (Exception ex)
            {
                Notify?.Invoke(ex.Message);
            }
        }


    }
}
