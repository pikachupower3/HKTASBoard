using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using TASBoardConsole.Definitions;
using TASBoardConsole.Renderers;

namespace TASBoardConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            var movieFile = "";
            var videoFile = "";
            if (!(args.Contains("-s") && args.Contains("-d")))
            {
                Console.WriteLine("Please provide a movie file '-s [path]' and a video destination '-d [path]");
                return;
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-s")
                    {
                        movieFile = args[++i];
                    }
                    else if (args[i] == "-d")
                    {
                        videoFile = args[++i];
                    }
                }
                if (File.Exists(videoFile))
                {
                    Console.WriteLine($"{videoFile} already exists, are you sure you want to overwrite it?[y/n]");
                    string input = Console.ReadLine();
                    if (!(input == "y" || input == "yes"))
                    {
                        return;
                    }
                }
                if (!File.Exists(movieFile))
                {
                    Console.WriteLine($"{movieFile} does not exist, please provide an existing file.");
                    return;
                }
            }
            // Console.WriteLine($"{movieFile} {videoFile}");

            var actLeft = NewAction(0, 1, "LEFT");
            SetPressedImage(actLeft, "Images/left_pressed.png");
            SetUnpressedImage(actLeft, "Images/left_unpressed.png");

            var actRight = NewAction(2, 1, "RIGHT");
            SetPressedImage(actRight, "Images/right_pressed.png");
            SetUnpressedImage(actRight, "Images/right_unpressed.png");

            var actUp = NewAction(1, 0, "UP");
            SetPressedImage(actUp, "Images/up_pressed.png");
            SetUnpressedImage(actUp, "Images/up_unpressed.png");

            var actDown = NewAction(1, 1, "DOWN");
            SetPressedImage(actDown, "Images/down_pressed.png");
            SetUnpressedImage(actDown, "Images/down_unpressed.png");

            var actJump = NewAction(3, 1, "Z");
            SetPressedImage(actJump, "Images/jump_pressed.png");
            SetUnpressedImage(actJump, "Images/jump_unpressed.png");

            var actSlash = NewAction(3, 0, "X");
            SetPressedImage(actSlash, "Images/nail_pressed.png");
            SetUnpressedImage(actSlash, "Images/nail_unpressed.png");

            var actDash = NewAction(4, 1, "C");
            SetPressedImage(actDash, "Images/dash_pressed.png");
            SetUnpressedImage(actDash, "Images/dash_unpressed.png");

            var actSpell = NewAction(4, 0, "F");
            SetPressedImage(actSpell, "Images/spell_pressed.png");
            SetUnpressedImage(actSpell, "Images/spell_unpressed.png");

            var actCDash = NewAction(5, 1, "S");
            SetPressedImage(actCDash, "Images/cdash_pressed.png");
            SetUnpressedImage(actCDash, "Images/cdash_unpressed.png");

            var actDreamnail = NewAction(5, 0, "D");
            SetPressedImage(actDreamnail, "Images/dnail_pressed.png");
            SetUnpressedImage(actDreamnail, "Images/dnail_unpressed.png");

            var actFocus = NewAction(6, 0, "A");
            SetPressedImage(actFocus, "Images/focus_pressed.png");
            SetUnpressedImage(actFocus, "Images/focus_unpressed.png");

            var actInventory = NewAction(6, 1, "I");
            SetPressedImage(actInventory, "Images/inv_pressed.png");
            SetUnpressedImage(actInventory, "Images/inv_unpressed.png");

            var actEscape = NewAction(0, 0, "ESCAPE");
            SetPressedImage(actEscape, "Images/esc_pressed.png");
            SetUnpressedImage(actEscape, "Images/esc_unpressed.png");

            var actReturn = NewAction(2, 0, "RETURN");
            SetPressedImage(actReturn, "Images/return_pressed.png");
            SetUnpressedImage(actReturn, "Images/return_unpressed.png");

            IReadOnlyList<InputAction> keys = new[]
            {
                actEscape,
                actUp,
                actReturn,
                actSlash,
                actSpell,
                actDreamnail,
                actFocus,
                actLeft,
                actDown,
                actRight,
                actJump,
                actDash,
                actCDash,
                actInventory
            };

            var renderer = new KeyRenderer(keys);
            var generator = new VideoGenerator(new[] {renderer});
            generator.Generate(movieFile, videoFile);
        }

        static InputAction NewAction(int x, int y, params string[] keyNames)
        {
            return new()
            {
                X = x,
                Y = y,
                KeyNames = keyNames
            };
        }

        static void SetPressedImage(InputAction action, string imageName)
        {
            using (var stream = File.Open(imageName, FileMode.Open, FileAccess.Read))
            {
                var decoder = new PngDecoder();
                action.PressedImage = decoder.Decode<Rgba32>(Configuration.Default, stream);
            }
        }

        static void SetUnpressedImage(InputAction action, string imageName)
        {
            using (var stream = File.Open(imageName, FileMode.Open, FileAccess.Read))
            {
                var decoder = new PngDecoder();
                action.UnpressedImage = decoder.Decode<Rgba32>(Configuration.Default, stream);
            }
        }
    }
}
