using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class PlayListViewModel : BindableBase
    {
        public SongListModel SongListModel { get; set; } = new SongListModel();
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

        private List<SongListItem> _disPlayList;

        public List<SongListItem> DisPlayList
        {
            get { return _disPlayList; }
            set
            {
                _disPlayList = value;
                RaisePropertyChanged("DisPlayList");
            }
        }

        public void InitialPlayList(List<Song> songList)
        {
            DisPlayList= SongListModel.InitialSongList(songList);
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
