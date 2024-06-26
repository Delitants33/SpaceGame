using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;



namespace Space
{
    public static class SoundManager
    {
        public static Song ambience;
        public static SoundEffect starRecieve;
        public static SoundEffect death;
        public static SoundEffect Tie;
        public static SoundEffect launch;
        public static SoundEffect gameStart;

        public static void PlaySFX(SoundEffect sfx)
        {
            var sfxInstance = sfx.CreateInstance();
            if (sfx == Tie)
                sfxInstance.Volume = 0.1f;
            sfxInstance.Pitch = new Random().Next(7, 10) / 10f;
            sfxInstance.Play();
        }

        public static void PlaySong(Song song)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.3f;
            MediaPlayer.Play(song);
        }
    }
}
