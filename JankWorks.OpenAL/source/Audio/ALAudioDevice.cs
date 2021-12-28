﻿using System;
using System.IO;
using System.Numerics;

using JankWorks.Audio;

using JankWorks.Drivers.OpenAL.Audio.Decoders;
using JankWorks.Drivers.OpenAL.Native;

using static JankWorks.Drivers.OpenAL.Native.Functions;

namespace JankWorks.Drivers.OpenAL.Audio
{
    sealed class ALAudioDevice : AudioDevice
    {
        private IntPtr device;
        private IntPtr context;

        public ALAudioDevice(IntPtr device)
        {
            this.device = device;
            unsafe
            {
                var context = alcCreateContext(device, (int*)0);
                if(context == IntPtr.Zero)
                {
                    var error = alGetError();
                    throw new AudioException($"ALAudioDevice { error }");
                }
                else
                {
                    this.context = context;
                }
            }

            alcMakeContextCurrent(this.context);
            alDistanceModel(ALDistanceModel.LinearDistanceClamped);

            this.Volume = 1f;
            this.Position = Vector3.Zero;
            this.Velocity = Vector3.Zero;
            this.Orientation = new Orientation()
            {
                Direction = Vector3.UnitZ,
                Up = Vector3.UnitY
            };
        }

        public override float Volume 
        { 
            get
            {
                unsafe
                {
                    float value = default;
                    alGetListenerf(ALListenerf.Gain, &value);
                    return value;
                }
                
            }
            set
            {
                alListenerf(ALListenerf.Gain, value);
            }
        }

        public override Vector3 Position 
        {
            get
            {                
                unsafe
                {
                    Vector3 value = default;
                    alGetListener3f(ALListener3f.Position, &value.X, &value.Y, &value.Z);
                    return value;                        
                }
            }
            set
            {
                alListener3f(ALListener3f.Position, value.X, value.Y, value.Z);
            }
        }

        public override Vector3 Velocity 
        { 
            get
            {
                unsafe
                {
                    Vector3 value = default;
                    alGetListener3f(ALListener3f.Velocity, &value.X, &value.Y, &value.Z);
                    return value;
                }
            }
            set
            {
                alListener3f(ALListener3f.Velocity, value.X, value.Y, value.Z);                
            }
        }

        public override Orientation Orientation 
        { 
            get
            {
                unsafe
                {
                    Orientation value = default;
                    alGetListenerfv(ALListenerfv.Orientation, (float*)&value);
                    return value;
                }
            }
            set
            {                
                unsafe
                {
                    alListenerfv(ALListenerfv.Orientation, (float*)&value);
                }
            }
        }

        public override Emitter CreateEmitter(Sound sound) => new ALEmitter(sound);

        public override Sound CreateSound(ReadOnlySpan<byte> pcm, short channels, short samples, int frequency)
        {
            var sound = new ALSound();
            sound.Write(pcm, channels, samples, frequency);
            return sound;
        }

        public override Sound LoadSound(Stream stream, AudioFormat format)
        {
            var decoder = Decoder.GetDecoder(format);
            var sound = new ALSound();

            try
            {
                decoder.Load(stream, sound.buffer);
            }
            catch
            {
                sound.Dispose();
                throw;
            }

            return sound;           
        }

        public override Sound LoadSound(ReadOnlySpan<byte> data, AudioFormat format)
        {
            var decoder = Decoder.GetDecoder(format);
            var sound = new ALSound();

            try
            {
                decoder.Load(data, sound.buffer);
            }
            catch
            {
                sound.Dispose();
                throw;
            }

            return sound;
        }

        protected override void Dispose(bool finalising)
        {
            alcDestroyContext(this.context);
            alcCloseDevice(this.device);

            base.Dispose(finalising);
        }
    }
}