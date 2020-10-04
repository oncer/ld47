using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SentIO.Globals
{
    public static class SoundManager
    {
        public static float SoundVolume { get; set; } = 1.0f;

        private static Dictionary<string, SoundEffectInstance> soundInst = new Dictionary<string, SoundEffectInstance>();

        public static bool IsPlaying(SoundEffect sound)
        {
            if (soundInst.ContainsKey(sound.Name) && soundInst[sound.Name].State == SoundState.Playing)
                return true;

            return false;
        }

        public static void Stop(SoundEffect sound)
        {
            if (IsPlaying(sound))
            {
                soundInst[sound.Name].Stop();
            }
        }

        public static void Play(SoundEffect sound, double pitch = 0, double pan = 0, bool skipPlaying = false, bool isLooped = false)
        {
            if (!soundInst.ContainsKey(sound.Name))
            {
                soundInst.Add(sound.Name, sound.CreateInstance());
            }

            if (IsPlaying(sound))
            {
                if (skipPlaying)
                    return;
                soundInst[sound.Name].Stop();
            }

            soundInst[sound.Name].Pitch = (float)pitch;
            soundInst[sound.Name].Pan = (float)pan;
            soundInst[sound.Name].IsLooped = isLooped;
            soundInst[sound.Name].Play();

            //sound.Play(SoundVolume, pitch, pan);
        }
    }
}
