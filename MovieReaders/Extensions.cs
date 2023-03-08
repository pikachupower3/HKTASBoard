using System.Collections.Generic;
using System.Linq;

namespace TASBoardConsole.MovieReaders
{
    public static class Extensions
    {
        public static InputFrame Sum(this IEnumerable<InputFrame> inputFrames)
        {
            return inputFrames.Aggregate((a, b) => a + b);
        }
    }
}
