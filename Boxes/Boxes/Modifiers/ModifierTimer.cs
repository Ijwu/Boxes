using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace Boxes.Modifiers
{
    public class ModifierTimer : IDisposable
    {
        private Timer _timer = new Timer();
        private double _min;
        private double _max;
        private Random _random = new Random();


        //private readonly Dictionary<Modifier, float> Weights = new Dictionary<Modifier, float>()
        //{
        //    {Modifier.Pull, .65f},
        //    {Modifier.Push, .50f}
        //}; 

        private readonly Dictionary<Modifier, float> Weights = new Dictionary<Modifier, float>()
        {
            {Modifier.Pull, .60f},
            {Modifier.Push, .10f},
            {Modifier.RandomizeGravity, .05f},
            {Modifier.GravityDown, .05f},
            {Modifier.GravityLeft, .05f},
            {Modifier.GravityRight, .05f},
            {Modifier.GravityUp, .05f},
            {Modifier.NoGravity, .05f}
        }; 

        public event ModifierElapsedEvent Elapsed;

        public ModifierTimer(double time)
        {
            _min = time;
            _max = time;

            _timer.Elapsed += OnTimerElapsed;
        }

        public ModifierTimer(double min, double max)
        {
            _min = min;
            _max = max;

            _timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Interval = _random.NextDouble()*_max + _min;
            Debug.WriteLine(_timer.Interval);
            OnModifierTimerElapsed(this, new ModifierElapsedEventArgs(ChooseModifier()));
        }

        private Modifier ChooseModifier()
        {
            double rnd = _random.NextDouble();
            var selectedModifier = Modifier.Pull;
            foreach (var key in Weights.Keys)
            {
                if (rnd < Weights[key])
                {
                    selectedModifier = key;
                    break;
                }
                rnd -= Weights[key];
            }
            if (selectedModifier == Modifier.Push)
            {
                _timer.Interval = 100;
            }
            return selectedModifier;
        }

        private void OnModifierTimerElapsed(object sender, ModifierElapsedEventArgs args)
        {
            if (Elapsed != null)
                Elapsed(sender, args);
        }
        
        public void Run()
        {
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        public void AdjustTimeRange(double min, double max)
        {
            _min = min;
            _max = max;
        }
    }

    public delegate void ModifierElapsedEvent(object sender, ModifierElapsedEventArgs args);

    public class ModifierElapsedEventArgs : EventArgs
    {
        public Modifier Modifier;

        public ModifierElapsedEventArgs(Modifier modifier)
        {
            Modifier = modifier;
        }
    }
}