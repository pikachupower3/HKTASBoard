using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using TASBoardConsole.Definitions;
using TASBoardConsole.MovieReaders;
using TASBoardConsole.Utils;

namespace TASBoardConsole.Renderers
{
    public sealed class KeyRenderer : IRenderer
    {

        private readonly KeyRenderInfo[] _keys;
        private readonly IReadOnlyList<InputAction> Keys;

        public KeyRenderer(IReadOnlyList<InputAction> keys)
        {
            Keys = keys;

            _keys = new KeyRenderInfo[keys.Count];
            for (var i = 0; i < keys.Count; i++)
            {
                var info = new KeyRenderInfo(keys[i]);
                _keys[i] = info;
            }
        }

        public void RenderFrame(Image target, TimeInterval currentTime)
        {
            int dim = 131;

            //Each lane can be drawn independently using their precomputed bounds
            foreach (var key in _keys)
            {
                Image keyImage = null;
                bool same = false;

                foreach (var entry in key.Entries)
                {
                    //Entries before the visible range can be ignored entirely
                    if (entry.Interval.End <= currentTime.Start)
                        continue;

                    //Once entries are beyond the visible range, we're done with this lane
                    if (entry.Interval.Start >= currentTime.End)
                        break;

                    if (key.PressedPrevFrame)
                    {
                        same = true;
                    }

                    keyImage = key.Key.PressedImage;
                    key.dtSinceRelease = key.DidPressPrevFrame ? 0 : 10;
                    key.PressedPrevFrame = true;
                    key.DidPressPrevFrame = true;

                }

                if (keyImage == null && key.dtSinceRelease > 0)
                {
                    same = true;
                    key.dtSinceRelease--;
                }

                if (!same)
                {
                    if (!key.PressedPrevFrame && key.didFirstRender)
                        goto SkiptToEnd;

                    if (key.PressedPrevFrame && key.didFirstRender)
                    {
                        var emptyRect = new RectangleF(dim * key.Key.X, dim * key.Key.Y, dim, dim);
                        target.Mutate(o => o
                                .Clear(new Bgra32(0, 0, 0, 0), emptyRect)
                        );
                    }

                    if (keyImage == null)
                    {
                        keyImage = key.Key.UnpressedImage;
                        key.PressedPrevFrame = false;
                        key.DidPressPrevFrame = false;
                    }

                    target.Mutate(o => o
                            .DrawImage(keyImage, new Point(dim * key.Key.X, dim * key.Key.Y), 1f)
                    );

                    key.didFirstRender = true;

                    SkiptToEnd:;
                }
            }
        }

        public void LoadInputs(IReadOnlyList<InputFrame> frames)
        {
            for (var i = 0; i < Keys.Count; i++)
            {
                var key = Keys[i];
                _keys[i].Entries = BuildKeyEntries(frames, key);
            }
        }

        private IReadOnlyList<Entry> BuildKeyEntries(IReadOnlyList<InputFrame> frames, InputAction key)
        {
            //We'll start by creating a separate event sequence for each input action
            //These can contain both isolated events and dense sequences
            var actionEntries = new Queue<InputEntry>[1];
            actionEntries[0] = BuildActionEntries(frames, key);

            //Build the final list of entries
            //While doing this, check for overlapping actions
            //and consolidate these into parallel splits
            var keyEntries = new List<Entry>();
            var pending = new List<InputEntry>();
            while (pending.Count > 0 || actionEntries.Any(q => q.Count > 0))
            {
                //Grab the next action for each entry queue, ordered by time
                if (pending.Count == 0)
                {
                    pending.AddRange(actionEntries.Where(q => q.Count > 0).Select(q => q.Dequeue())
                                                  .OrderBy(e => e.Interval.Start));
                }

                //Entries don't belong to a prior split, so now we can add individual entries from pending while checking for a new split
                while (pending.Count > 0)
                {
                    keyEntries.Add(pending[0]);
                    pending.RemoveAt(0);
                }
            }

            keyEntries.Sort((a, b) => Math.Sign(a.Interval.Start - b.Interval.Start));
            return keyEntries;
        }

        private Queue<InputEntry> BuildActionEntries(IReadOnlyList<InputFrame> frames, InputAction action)
        {
            var isPressed = frames.Select(f => action.Matches(f.Inputs)).ToArray();
            var entries = new Queue<InputEntry>();

            for (int i = 0; i < isPressed.Length; i++)
            {
                if (isPressed[i])
                {
                    var entry = new InputEntry(frames[i].Interval, action);
                    entries.Enqueue(entry);
                }
            }

            return entries;
        }

        private abstract class Entry
        {
            protected Entry(TimeInterval interval)
            {
                Interval = interval;
            }

            public TimeInterval Interval { get; }
        }

        private sealed class InputEntry : Entry
        {
            public InputEntry(TimeInterval interval, InputAction input)
            : base(interval)
            {
                Input = input;
            }

            public InputAction Input { get; }
        }

        private sealed class KeyRenderInfo
        {
            public KeyRenderInfo(InputAction action)
            {
                Key = action;
            }

            public int dtSinceRelease { get; set; } = 0;

            public bool PressedPrevFrame { get; set; } = false;
            
            public bool DidPressPrevFrame { get; set; } = false;

            public bool didFirstRender { get; set;  } = false;

            public InputAction Key { get; }

            public IReadOnlyList<Entry> Entries { get; set; }
        }
    }
}

