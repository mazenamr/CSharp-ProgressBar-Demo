using static System.Math;

namespace ProgressBar
{
    internal class ProgressBarConfig
    {
        public int TimeStep { get; private set; } = 500;
        
        public void UpdateTimeStep(int increment)
        {
            TimeStep = Max(Min(TimeStep + increment, 5000), 100);
        }

        public char Character { get; private set; } = '#';

        public void SetChar(char newChar)
        {
            Character = newChar;
        }

        public int BarLength { get; private set; } = 60;

        public void UpdateBarLength(int increment)
        {
            BarLength = Max(Min(BarLength + increment, 100), 20);
        }

        public bool Done { get; private set; } = false;

        public void SetDone()
        {
            Done = true;
        }

        public int Progress { get; private set; } = 0;
        
        public void IncProgress()
        {
            if (Progress < 100)
            {
                Progress += 1;
            }
            else
            {
                SetDone();
            }
        }
    }
}