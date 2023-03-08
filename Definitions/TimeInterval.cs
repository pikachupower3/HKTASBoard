namespace TASBoardConsole.Definitions
{
    public readonly struct TimeInterval
    {
        public TimeInterval(double start, double end)
        {
            Start = start;
            End = end;
        }

        public double Start { get; }

        public double End { get; }
    }
}