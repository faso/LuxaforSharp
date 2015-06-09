﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HidLibrary;
using LuxaforSharp.Commands;

namespace LuxaforSharp
{
    public abstract class BaseDevice : IDevice, IDisposable
    {
        /*
         * Commands not yet implemented :
         * private const byte CommandGetStatus = 0x80;
         * private const byte CommandToggleLed = 0x81;
         */

        public abstract void Dispose();
        public abstract Task<bool> SendCommand(ICommand command, int timeout = 0);

        public IPort AllLeds
        {
            get { return new LedPort(this, LedTarget.All); }
        }

        public IPort AllFrontsideLeds
        {
            get { return new LedPort(this, LedTarget.AllFrontSide); }
        }

        public IPort AllBacksideLeds
        {
            get { return new LedPort(this, LedTarget.AllBackSide); }
        }

        public IPort this [byte index]
        {
            get { return new LedPort(this, LedTarget.OfIndex(index)); }
        }
        
        public Task<bool> SetColor(LedTarget target, Color color, byte? fadeInTime = null, int timeout = 0)
        {
            var command = fadeInTime.HasValue
                ? (ICommand) new FadeToCommand(target, color, fadeInTime.Value)
                : (ICommand) new JumpToCommand(target, color);

            return SendCommand(command, timeout);
        }

        public Task<bool> Flash(LedTarget target, Color color, byte speed, byte repeatCount = 0, int timeout = 0)
        {
            var command = new FlashCommand(target, color, speed, repeatCount);
            return SendCommand(command, timeout);
        }

        public Task<bool> Wave(WaveType waveType, Color color, byte speed, byte repeatCount, int timeout = 0)
        {
            var command = new WaveCommand(waveType, color, speed, repeatCount);
            return SendCommand(command, timeout);
        }

        public Task<bool> CarryOutPattern(PatternType patternType, byte repeatCount, int timeout = 0)
        {
            var command = new PatternCommand(patternType, repeatCount);
            return SendCommand(command, timeout);
        }
    }
}
