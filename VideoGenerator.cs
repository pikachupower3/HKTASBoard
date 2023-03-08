using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using FFmpeg.AutoGen;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TASBoardConsole.Definitions;
using TASBoardConsole.MovieReaders;
using TASBoardConsole.Renderers;

namespace TASBoardConsole
{
    public sealed class VideoGenerator
    {
        static bool FFmpegPathSet = false;

        private readonly IReadOnlyList<IRenderer> _renderers;

        public VideoGenerator(IReadOnlyList<IRenderer> renderers)
        {
            _renderers = renderers;
        }

        public Fraction Framerate { get; } = new Fraction(60, 1);

        public void Generate(string tasFilename, string videoFilename)
        {
            int dim = 131;

            var settings = new VideoEncoderSettings(width: dim * 7, height: dim * 2, codec: VideoCodec.H264)
            {
                EncoderPreset = EncoderPreset.Fast,
                CRF = 10,
                FramerateRational = (AVRational)Framerate,
                VideoFormat = ImagePixelFormat.Yuv444
            };

            // Set the ffmpeg path if not already set
            if (!FFmpegPathSet && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FFmpegLoader.FFmpegPath = Path.GetFullPath("ffmpeg/x86_64");
                FFmpegPathSet = true;
            }

            var movieReader = new LibTASReader(tasFilename);
            var inputFrames = movieReader.ToList();
            if (inputFrames.Count == 0)
            {
                return;
            }

            var imgBytes = new byte[settings.VideoWidth*settings.VideoHeight*4];
            var imgMemory = new Memory<byte>(imgBytes);
            using (var file = MediaBuilder.CreateContainer(videoFilename).WithVideo(settings).Create())
            using (var image = Image.WrapMemory<Bgra32>(imgMemory, settings.VideoWidth, settings.VideoHeight))
            {
                foreach (var renderer in _renderers)
                {
                    renderer.LoadInputs(inputFrames);
                }

                var endTime = inputFrames[^1].Interval.End;
                var lastVideoFrame = (int)Math.Round((endTime*Framerate.Num)/Framerate.Den);
                var dt = (double) Framerate.Den/Framerate.Num;
                for (int i = 0; i < lastVideoFrame; i++)
                {
                    Console.Write($"Encoding video frame {i+1} of {lastVideoFrame}...\r");
                    var t = (double)(i*Framerate.Den)/Framerate.Num;
                    var currentTime = new TimeInterval(t, t + dt);

                    //Render all elements
                    foreach (var renderer in _renderers)
                    {
                        renderer.RenderFrame(image, currentTime);
                    }

                    var videoFrame = ImageData.FromArray(imgBytes, ImagePixelFormat.Bgra32, settings.VideoWidth, settings.VideoHeight);
                    file.Video.AddFrame(videoFrame);
                }
            }
        }
    }
}
