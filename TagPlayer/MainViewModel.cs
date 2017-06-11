using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagPlayer.Model;
using Prism.Mvvm;
using System.Windows.Media;
using TagPlayer.ViewModels;
using System.Collections.ObjectModel;

namespace TagPlayer
{
    public class MainViewModel : BindableBase
    {
        public SongListModel SongListModel { get; set; } = new SongListModel();

        private List<Song> _songList;

        public List<Song> SongList
        {
            get { return _songList; }
            set
            {
                _songList = value;
                OnSongListChanged();
            }
        }

        private Song _playingSong;

        public Song PlayingSong
        {
            get { return _playingSong; }
            set { _playingSong = value; }
        }

        private void OnSongListChanged()
        {
            if (SongListViewModel != null)
            {
                SongListViewModel.InitialSongList(SongList);
            }
        }

        public TagsPanelViewModel TagsPanelViewModel { get; set; }
        public SongListViewModel SongListViewModel { get; set; }
        public ControlViewModel ControlViewModel { get; set; }
        public PlayListViewModel PlayListViewModel { get; set; }

        public MainViewModel()
        {
            SongListViewModel = new SongListViewModel();

            SongList = new List<Song>();
            TagsPanelViewModel = new TagsPanelViewModel(this);
            ControlViewModel = new ControlViewModel();
            PlayListViewModel = new PlayListViewModel();
        }
    }

    public class SongListItem
    {
        public Song Song { get; set; }
        public int SongNum { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
