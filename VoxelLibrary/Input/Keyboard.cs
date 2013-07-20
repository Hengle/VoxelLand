using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;

namespace VoxelLand
{
    public class Keyboard
    {
        public Keyboard()
        {
            keyDownDurations = new Dictionary<Keys, TimeSpan>();
            keyDownTimes = new Dictionary<Keys, TimeSpan>();
            inputLock = new object();
            s = Stopwatch.StartNew();
        }

        public void OnKeyDown(Keys key)
        {
            lock (inputLock)
            {
                if (! keyDownTimes.ContainsKey(key))
                    keyDownTimes[key] = s.Elapsed;
            }
        }

        public void OnKeyUp(Keys key)
        {
            lock (inputLock)
            {
                if (! keyDownDurations.ContainsKey(key))
                    keyDownDurations[key] = TimeSpan.Zero;

                if (keyDownTimes.ContainsKey(key))
                {
                    keyDownDurations[key] = keyDownDurations[key] + (s.Elapsed - keyDownTimes[key]);
                    keyDownTimes.Remove(key);
                }
            }
        }

        public TimeSpan Peek(Keys key)
        {
            lock (inputLock)
            {
                TimeSpan total = TimeSpan.Zero;
                if (keyDownDurations.ContainsKey(key))
                    total += keyDownDurations[key];
                if (keyDownTimes.ContainsKey(key))
                    total += (s.Elapsed - keyDownTimes[key]);
                return total;
            }
        }

        public TimeSpan Read(Keys key)
        {
            lock (inputLock)
            {
                TimeSpan total = TimeSpan.Zero;
                if (keyDownDurations.ContainsKey(key))
                {
                    total += keyDownDurations[key];
                    keyDownDurations.Remove(key);
                }
                if (keyDownTimes.ContainsKey(key))
                {
                    TimeSpan now = s.Elapsed;
                    keyDownDurations[key] = now - keyDownTimes[key];
                    total += keyDownDurations[key];
                    keyDownTimes[key] = now;
                }
                return total;
            }
        }

        private object inputLock;
        private Dictionary<Keys, TimeSpan> keyDownDurations;
        private Dictionary<Keys, TimeSpan> keyDownTimes;
        private Stopwatch s;
    }
}
