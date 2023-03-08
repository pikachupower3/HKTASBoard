using System;
using System.Collections.Generic;
using System.Linq;
using TASBoardConsole.Definitions;

namespace TASBoardConsole.MovieReaders
{
    public readonly struct InputFrame
    {
        public readonly List<string> Inputs;
        public readonly Fraction TimeDelta;
        public readonly TimeInterval Interval;

        public InputFrame(List<string> inputs, Fraction timeDelta, TimeInterval interval)
        {
            Inputs = inputs;
            TimeDelta = timeDelta;
            Interval = interval;
        }

        public static InputFrame operator + (InputFrame a, InputFrame b)
        {
            return new InputFrame(inputs: a.Inputs.Union(b.Inputs).ToList(), timeDelta: a.TimeDelta + b.TimeDelta,
                                  new TimeInterval(Math.Min(a.Interval.Start, b.Interval.Start),
                                                   Math.Max(a.Interval.End, b.Interval.End)));
        }
        
        public static InputFrame operator + (InputFrame a, Fraction b)
        {
            return new InputFrame(inputs: a.Inputs, timeDelta: a.TimeDelta + b,
                                  new TimeInterval(a.Interval.Start, a.Interval.End + (float)b.Num/b.Den));
        }
    }
}