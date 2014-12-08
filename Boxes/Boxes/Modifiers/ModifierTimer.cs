using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Microsoft.Xna.Framework;

namespace Boxes.Modifiers
{
    public class ModifierTimer : IDisposable
    {
        private Timer _timer = new Timer();
        private double _min;
        private double _max;
        private Random _random = new Random();
        private Modifier _lastModifier;


        //private readonly Dictionary<Modifier, float> Weights = new Dictionary<Modifier, float>()
        //{
        //    {Modifier.Pull, .65f},
        //    {Modifier.Push, .50f}
        //}; 

        private readonly Dictionary<Modifier, float> Weights = new Dictionary<Modifier, float>()
        {
            {Modifier.GravityDown, .40f},
            {Modifier.GravityLeft, .15f},
            {Modifier.GravityRight, .15f},
            {Modifier.GravityUp, .10f},
            {Modifier.NoGravity, .20f}
        };

        private readonly Dictionary<Modifier, float> AlternateWeights = new Dictionary<Modifier, float>()
        {
            {Modifier.GravityDown, .80f},
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
            ModifierElapsedEventArgs thing;
            if (_lastModifier != Modifier.GravityDown)
                thing = new ModifierElapsedEventArgs(ChooseAlternateModifier());
            else
                thing = new ModifierElapsedEventArgs(ChooseModifier());
            OnModifierTimerElapsed(this, thing);
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

        private Modifier ChooseAlternateModifier()
        {
            double rnd = _random.NextDouble();
            var selectedModifier = Modifier.Pull;
            foreach (var key in AlternateWeights.Keys)
            {
                if (rnd < AlternateWeights[key])
                {
                    selectedModifier = key;
                    break;
                }
                rnd -= AlternateWeights[key];
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

        public void AdjustTimeRange(double time)
        {
            _min = time;
            _max = time;
        }

        public Vector2 GetTimeRange()
        {
            return new Vector2((float)_min, (float)_max);
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