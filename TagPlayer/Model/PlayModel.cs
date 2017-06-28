using System;
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

            MediaPlayer = new MediaElement()
            {
                UnloadedBehavior = MediaState.Manual
            };
            MediaPlayer.MediaEnded += Media_End;
        }

        public Action MediaEnd { get; set; }

        private void Media_End(object sender, RoutedEventArgs e)
        {
            MediaEnd();
        }

        public PlayState PlayState { get; set; }
        public Timer Timer { get; set; }
        public MediaElement MediaPlayer { get; set; }
        private bool _isDrag;

        public bool IsDrag
        {
            get { return _isDrag; }
            set
            {
                _isDrag = value;
                if (!_isDrag)
                {
                    OnChangePeriod();
                }
            }
        }
        private bool _isChanged;

        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                if (_isChanged)
                {
                    OnChangePeriod();
                    _isChanged = false;
                }
            }
        }

        public Action OnChangePeriod { get; set; }

        public void Play(string path)
        {
            if (File.Exists(path))
            {
                PlayState = PlayState.播放;
                MediaPlayer.Source = (new Uri(path, UriKind.Absolute));
                MediaPlayer.Play();
            }
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

        /// <summary> 播放歌曲的总时长 </summary>
        public double Duration { get; set; }

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
