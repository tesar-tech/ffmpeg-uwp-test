using FFmpegInterop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DalsiTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //base project https://github.com/ffmpeginteropx/FFmpegInteropX
        //builded nuget https://www.nuget.org/packages/Hyvart.FFmpegInteropX/
        //ffmpeg framegrabber ffmpeg -i beru.wmv -r 25 $filename%03d.bmp
        public MainPage()
        {
            this.InitializeComponent();
            sw = new Stopwatch();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetSF();//initialize file, stream and frame grabber
        }

        private StorageFile currentFile;
        private IRandomAccessStream stream;
        FrameGrabber frameGrabber;

        private async void GetSF()
        {
            currentFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/beru.wmv"));
            stream = await currentFile.OpenAsync(FileAccessMode.Read);
            frameGrabber = await FrameGrabber.CreateFromStreamAsync(stream);
        }

        private async Task ExtractFrame(TimeSpan position)
        {
            //await Task.Delay(1000);
                //var sws = new Stopwatch();
                //sws.Start();
                var frame = await frameGrabber.ExtractVideoFrameAsync(position, true);
                //Debug.WriteLine(sws.ElapsedMilliseconds);
            //read data:
            //var pixData = frame.PixelData.ToArray();
            //Debug.WriteLine(pixData[10500]);
        }

        Stopwatch sw;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();
            sw.Start();
            int numOfFrames = (int)Math.Floor(mpe.MediaPlayer.PlaybackSession.NaturalDuration.TotalSeconds * 25);
            TimeSpan currentPosition = TimeSpan.Zero;

            for (int i = 0; i < numOfFrames; i++)
            {
                currentPosition += TimeSpan.FromMilliseconds(1000d / 25d);//every frame
                await ExtractFrame(currentPosition);
            }
            Debug.WriteLine($"All ({numOfFrames}) in {sw.Elapsed.TotalSeconds} seconds");
        }
    }

   
}
