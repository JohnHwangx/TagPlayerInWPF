﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TagPlayer.Model
{
    public class PlayModel : DispatcherObject
    {
        private static PlayModel _instance;
        public static PlayModel Instance
        {
            get { return _instance ?? (_instance = new PlayModel()); }
        }

        private PlayModel()
        {
            Timer = new Timer();
            Timer.Elapsed += Timer_Elapsed;
            Timer.Interval = 100;
            Timer.Start();

            MediaPlayer = new MediaElement();
            //MediaPlayer.LoadedBehavior = MediaState.Manual;
            MediaPlayer.UnloadedBehavior = MediaState.Manual;
            MediaPlayer.MediaEnded += Media_End;
        }

        public Action MediaEnd { get; set; }

        private void Media_End(object sender, RoutedEventArgs e)
        {
            MediaEnd();
        }

        //public string SongPath { get; set; }
        public PlayState PlayState { get; set; }
        public Timer Timer { get; set; }
        public MediaElement MediaPlayer { get; set; }
        //public bool IsDrag { get; set; }

        public void Play(string path)
        {
            //if (MediaPlayer.Source == null)
            {
                if (File.Exists(path))
                {
                    PlayState = PlayState.播放;
                    MediaPlayer.Source = (new Uri(path, UriKind.Absolute));
                    MediaPlayer.Play();
                }
            }
            //else
            //{
            //    PlayState = PlayState.播放;
            //    MediaPlayer.Play();
            //}
        }
        public void Play()
        {
            PlayState = PlayState.播放;
            MediaPlayer.Play();
        }

        public void Pause()
        {
            PlayState = PlayState.暂停;
            MediaPlayer.Pause();
        }

        /// <summary> 用于显示的时长 </summary>
        //public string Rate { get; set; }
        /// <summary> 播放歌曲的总时长 </summary>
        public double Duration { get; set; }
        private double _period;

        /// <summary> 当前进度,对应Slider的Value </summary>
        public double Period
        {
            get { return _period; }
            set
            {
                _period = value;
                //Rate = GetSongDuration(_period);
            }
        }

        public MediaState MediaState { get; set; }


        /// <summary>
        /// 根据进度条值计算歌曲进度，以mm:ss格式显示
        /// </summary>
        private string GetSongDuration(double period)
        {
            var songDuration = period / 500.0 * Duration;
            return ((int)songDuration / 60 < 10 ? "0" + (int)songDuration / 60 : ((int)songDuration / 60).ToString()) +
                   " : " +
                   ((int)songDuration % 60 < 10 ? "0" + (int)songDuration % 60 : ((int)songDuration % 60).ToString());
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (PlayState == PlayState.播放)
            {
                Dispatcher.Invoke(SetPrograssBar);
            }
        }

        public Action SetPrograssBar;
    }
}
