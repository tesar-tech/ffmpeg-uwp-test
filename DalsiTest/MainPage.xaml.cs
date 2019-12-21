using FFmpegInterop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
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

        //frame grabber sample: https://github.com/ffmpeginteropx/FFmpegInteropX/blob/3d0e2cfd6c64f67b339637e27b543bb5d95398fc/Tests/TestExtractThumbnail.cs

        private StorageFile currentFile;
        private IRandomAccessStream stream;
        FrameGrabber frameGrabber;
        Stopwatch sw;


        public MainPage()
        {
            this.InitializeComponent();
            sw = new Stopwatch();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //initialize file, stream and frame grabber
            currentFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/beru.wmv"));
            stream = await currentFile.OpenAsync(FileAccessMode.Read);
            frameGrabber = await FrameGrabber.CreateFromStreamAsync(stream);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            sw.Restart();
            int numOfFrames = (int)Math.Floor(mpe.MediaPlayer.PlaybackSession.NaturalDuration.TotalSeconds * 25);
            TimeSpan currentPosition = TimeSpan.Zero;

            for (int i = 0; i < numOfFrames; i++)
            {
                currentPosition += TimeSpan.FromMilliseconds(1000d / 25d);//every frame (fps is 25 for test video)
                var frame = await frameGrabber.ExtractVideoFrameAsync(currentPosition, true);
                Debug.WriteLine($"Frame from {currentPosition.TotalSeconds} sec. Elapsed:{sw.Elapsed.TotalSeconds} seconds");
            }
            Debug.WriteLine($"All ({numOfFrames}) in {sw.Elapsed.TotalSeconds} seconds");
        }
    }


}
