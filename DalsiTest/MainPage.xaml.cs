//using Hyv = FFmpegInterop;
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
using FFmpegInterop;


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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameGrabber frameGrabber;
            frameGrabber = await FrameGrabber.CreateFromStreamAsync(stream);
            sw.Restart();
            int counter = 0;
            VideoFrame frame = await frameGrabber.ExtractVideoFrameAsync(TimeSpan.Zero, true);
            do
            {
                frame = await frameGrabber.ExtractNextVideoFrameAsync();
                Debug.WriteLine($"Frame (#{counter++}) from {frame?.Timestamp.TotalSeconds} sec. Elapsed:{sw.Elapsed.TotalSeconds} seconds");


            } while (frame != null) ;

            Debug.WriteLine($"All ({counter}) in {sw.Elapsed.TotalSeconds} seconds");
    }
     }

    }
