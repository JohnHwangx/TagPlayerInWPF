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
        private Song PlayingSong { get; set; }

        //private bool _isPlay;

        //public bool IsPlay
        //{
        //    get { return _isPlay; }
        //    set
        //    {
        //        _isPlay = value;
        //        RaisePropertyChanged("IsPlay");
        //    }
        //}

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
                //Period = mediaPlayer.Position.TotalSeconds / MainViewModel.PlayingSong.Duration * 500.0;
            }
        }

        public ControlViewModel(MainViewModel mainViewModel)
        {
            //IsPlay = false;
            MainViewModel = mainViewModel;
            PlayingSongOperator = new PlayingSongOperator();

            PlayPauseCommand = new DelegateCommand(OnPlayPause);
            NextCommand = new DelegateCommand(OnNext);
            LastCommand = new DelegateCommand(OnLast);
            PlayModeChangeCommand = new DelegateCommand(OnPlayModeChanged);
        }
    }
}
