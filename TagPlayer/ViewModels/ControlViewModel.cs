using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TagPlayer.Model;
using System.Windows.Input;
using Prism.Commands;

namespace TagPlayer.ViewModels
{
    public class ControlViewModel : BindableBase
    {
        public PlayingSongOperator PlayingSongOperator { get; set; }
        /// <summary>
        /// 为了改变PlayingSong而加入
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        private PlayMode _playMode;

        public PlayMode PlayMode
        {
            get { return _playMode; }
            set
            {
                _playMode = value;
                RaisePropertyChanged("PlayMode");
            }
        }

        private string _rate;
        /// <summary> 用于显示的时长 </summary>
        public string Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                RaisePropertyChanged("Rate");
            }
        }

        public ICommand PlayPauseCommand { get; set; }

        private void OnPlayPause()
        {
            //IsPlay = !IsPlay;
            if (MainViewModel.PlayState!=PlayState.播放)
            {
                MainViewModel.PlayState = PlayState.播放;
            }
            else
            {
                MainViewModel.PlayState = PlayState.暂停;
            }
        }

        public ICommand PlayModeChangeCommand { get; set; }
        private void OnPlayModeChanged()
        {
            switch (PlayMode)
            {
                case PlayMode.LoopPlay:
                    PlayMode = PlayMode.SinglePlay;
                    break;
                case PlayMode.SinglePlay:
                    PlayMode = PlayMode.SequentialPlay;
                    break;
                case PlayMode.SequentialPlay:
                    PlayMode = PlayMode.RandomPlay;
                    break;
                case PlayMode.RandomPlay:
                    PlayMode = PlayMode.LoopPlay;
                    break;
                default:
                    break;
            }
        }

        public ICommand NextCommand { get; set; }
        private void OnNext()
        {
            MainViewModel.PlayingSong = PlayingSongOperator.OnNextExecute(PlayMode, MainViewModel.PlayList, MainViewModel.PlayList.IndexOf(MainViewModel.PlayingSong));
        }

        public ICommand LastCommand { get; set; }
        private void OnLast()
        {
            MainViewModel.PlayingSong = PlayingSongOperator.OnLastExecute(PlayMode, MainViewModel.PlayList, MainViewModel.PlayList.IndexOf(MainViewModel.PlayingSong));
        }

        public bool IsDrag { get; set; }

        private double _period;

        /// <summary> 当前进度,对应Slider的Value </summary>
        public double Period
        {
            get { return _period; }
            set
            {
                _period = value;
                Rate = GetSongDuration(_period);
                RaisePropertyChanged("Period");
            }
        }

        /// <summary>
        /// 根据进度条值计算歌曲进度，以mm:ss格式显示
        /// </summary>
        private string GetSongDuration(double period)
        {
            var songDuration = period / 500.0 * GetDuration(MainViewModel.PlayingSong.Duration);
            return ((int)songDuration / 60 < 10 ? "0" + (int)songDuration / 60 : ((int)songDuration / 60).ToString()) +
                   " : " +
                   ((int)songDuration % 60 < 10 ? "0" + (int)songDuration % 60 : ((int)songDuration % 60).ToString());
        }

        /// <summary>
        /// 设置进度条每秒的移动
        /// </summary>
        private void SetPrograssBar()
        {
            if (!IsDrag)
            {
                var mediaPlayer = PlayModel.Instance.MediaPlayer;
                int second = (int)mediaPlayer.Position.TotalSeconds % 60;
                int minute = (int)mediaPlayer.Position.TotalMinutes;
                //Rate = (minute < 10 ? "0" + minute : minute.ToString()) + " : " +
                //       (second < 10 ? "0" + second : second.ToString());
                Period = mediaPlayer.Position.TotalSeconds / GetDuration(MainViewModel.PlayingSong.Duration) * 500.0;
            }
        }

        private double GetDuration(string duration)
        {
            if (duration == null) return 0;
            var time = duration.Split(':');
            return int.Parse(time[0]) * 60 * 60 + int.Parse(time[1]) * 60 + int.Parse(time[2]);
        }

        public ControlViewModel(MainViewModel mainViewModel)
        {
            //IsPlay = false;
            MainViewModel = mainViewModel;
            PlayingSongOperator = new PlayingSongOperator();
            PlayModel.Instance.SetPrograssBar = SetPrograssBar;

            PlayPauseCommand = new DelegateCommand(OnPlayPause);
            NextCommand = new DelegateCommand(OnNext);
            LastCommand = new DelegateCommand(OnLast);
            PlayModeChangeCommand = new DelegateCommand(OnPlayModeChanged);
        }
    }
}
