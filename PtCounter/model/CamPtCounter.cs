using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AForge.Video;
using AForge.Video.DirectShow;

namespace PtCounter.model
{
    class CamPtCounter
    {
        //private FileVideoSource videoSource;
        //private VideoCaptureDevice camDevice;


        public readonly IVideoSource camSource;

        public string Name { get; }

        private DateTime lastChek;

        private ulong CountSum;

        private ulong LastCheckCountSum;

        private Rectangle rectangle;

        private readonly int timerDelay = 5000;

        private Timer timer;

        public delegate Task NewReport(ReportCount reportCount, string Moniker);
        public event NewReport sendNewReport;

        public CamPtCounter(IVideoSource camSource, string Name, Rectangle rectangle)
        {
            //videoSource = new FileVideoSource(camSource);
            //videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            //videoSource.PlayingFinished += new PlayingFinishedEventHandler(video_EndPlaying);

            //camDevice = new VideoCaptureDevice(camSource);
            //camDevice.NewFrame += new NewFrameEventHandler(video_NewFrame);
            //camDevice.PlayingFinished += new PlayingFinishedEventHandler(video_EndPlaying);

            this.Name = Name;

            this.rectangle = rectangle;

            this.camSource = camSource;

            this.camSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            this.camSource.PlayingFinished += new PlayingFinishedEventHandler(video_EndPlaying);

            CountSum = 0;
            LastCheckCountSum = 0;

            newTimer();

            //startTimer();
        }

        private void newTimer()
        {
            timer = new System.Timers.Timer(timerDelay);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = false;
        }
        
        public void StartTimer()
        {
            timer.Enabled = true;
        }

        public void StopTimer()
        {
            timer.Enabled = false;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            sendNewReport(GetCount(), camSource.Source);
        }

        public void StartCounting()
        {
            camSource.Start();
            StartTimer();
        }

        public void StopCounting()
        {
            camSource.SignalToStop();
            StopTimer();
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            int bcount = BitmapCounter.BitmapCount(bitmap, rectangle);

            CountSum += (ulong)bcount;

        }

        private void video_EndPlaying(object sender, ReasonToFinishPlaying eventArgs)
        {

        }

        public ReportCount GetCount()
        {
            int count = (int)(CountSum - LastCheckCountSum);
            long dateTicks = DateTime.Now.Ticks - lastChek.Ticks;

            LastCheckCountSum = CountSum;
            lastChek = DateTime.Now;

            return new ReportCount() { Count = count, Duration = new TimeSpan(dateTicks) , dateTime = DateTime.Now };
        }


    }
}
