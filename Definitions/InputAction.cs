using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace TASBoardConsole.Definitions
{
    public sealed class InputAction
    {
        public string Name { get; set; }

        public IReadOnlyList<string> KeyNames { get; set; } = new string[] { };

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public Image PressedImage { get; set; }
        
        public Image UnpressedImage { get; set; }

        public bool Matches(IReadOnlyList<string> keyNames)
        {
            return keyNames.Intersect(KeyNames).Count() == KeyNames.Count;
        }
    }
}
