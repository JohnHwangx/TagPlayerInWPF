using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TagPlayer.Model;

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

        public ControlViewModel()
        {

        }
    }
}
