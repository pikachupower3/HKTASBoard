using System.Collections.Generic;
using SixLabors.ImageSharp;
using TASBoardConsole.Definitions;
using TASBoardConsole.MovieReaders;

namespace TASBoardConsole.Renderers
{
    public interface IRenderer
    {
        void LoadInputs(IReadOnlyList<InputFrame> frames);

        void RenderFrame(Image target, TimeInterval currentTime);
    }
}