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

        public bool IsSongListChanged { get; set; }

        private List<Song> _songList;

        public List<Song> SongList
        {
            get { return _songList; }
            set
            {
                _songList = value;
                OnSongListChanged();
                IsSongListChanged = true;
            }
        }

        private List<Song> _playList;

        public List<Song> PlayList
        {
            get { return _playList; }
            set
            {
                _playList = value;
                OnPlayListChanged();
            }
        }

        private void OnPlayListChanged()
        {
            if (PlayListViewModel!=null)
            {
                PlayListViewModel.InitialPlayList(PlayList);
            }
        }

        private Song _playingSong;

        public Song PlayingSong
        {
            get { return _playingSong; }
            set
            {
                _playingSong = value;
                OnPlayingSongChanged();
            }
        }

        private void OnPlayingSongChanged()
        {
            
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
            SongListViewModel = new SongListViewModel(this);

            SongList = new List<Song>();
            PlayList = new List<Song>();
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
