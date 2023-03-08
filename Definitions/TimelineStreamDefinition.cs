using System.Collections.Generic;

namespace TASBoardConsole.Definitions
{
    public sealed class TimelineStreamDefinition
    {
        public TimelineStreamDefinition(IReadOnlyList<TimelineLaneDefinition> lanes)
        {
            Lanes = lanes;
        }

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public int Z { get; set; } = 0;

        public int Width { get; set; } = 7*32;

        public int Height { get; set; } = 800;

        public float PixelsPerSecond { get; set; } = 200f;

        public float MarginRatio { get; set; } = 0.0625f;

        public float CurrentTimePositionRatio { get; set; } = 0.5f;

        public float BumpWindowTime { get; set; } = 0.2f;

        public float BumpMinHoldTime { get; set; } = 0.04f;

        public float BumpDisplacementRatio { get; set; } = 0.15f;

        public float BumpEasingFactor { get; set; } = 6.0f;

        public IReadOnlyList<TimelineLaneDefinition> Lanes { get; }
    }
}
