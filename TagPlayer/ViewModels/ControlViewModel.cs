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
        private Song PlayingSong { get; set; }

        private bool _isPlay;

        public bool IsPlay
        {
            get { return _isPlay; }
            set
            {
                _isPlay = value;
                RaisePropertyChanged("IsPlay");
            }
        }

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
            IsPlay = !IsPlay;
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

        public ControlViewModel()
        {
            IsPlay = false;
            PlayPauseCommand = new DelegateCommand(OnPlayPause);
            PlayModeChangeCommand = new DelegateCommand(OnPlayModeChanged);
        }
    }
}
