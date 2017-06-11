using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;

namespace TagPlayer.ViewModels
{
    public class PlayListViewModel : BindableBase
    {
        private bool _isShowPlayList;

        public bool IsShowPlayList
        {
            get { return _isShowPlayList; }
            set
            {
                _isShowPlayList = value;
                RaisePropertyChanged("IsShowPlayList");
            }
        }

        public ICommand ShowPlayListCommand { get; set; }

        private void OnShowPlayList()
        {
            IsShowPlayList = !IsShowPlayList;
        }

        public PlayListViewModel()
        {
            IsShowPlayList = false;
            ShowPlayListCommand = new DelegateCommand(OnShowPlayList);
        }
    }
}
