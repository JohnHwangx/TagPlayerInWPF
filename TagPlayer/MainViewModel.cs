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
using Prism.Commands;
using System.Windows.Controls;

namespace TagPlayer
{
    public class MainViewModel : BindableBase
    {
        //public PlayingSongOperator PlayingSongOperator { get; set; } = new PlayingSongOperator();
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
            if (PlayListViewModel != null)
            {
                PlayListViewModel.InitialPlayList(PlayList);
                ControlViewModel.PlayingSongOperator.IsPlayingListChanged = true;
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
                RaisePropertyChanged("PlayingSong");
            }
        }

        private void OnPlayingSongChanged()
        {
            PlayingSong.LoadAlbum();
        }

        private void OnSongListChanged()
        {
            if (SongListViewModel != null)
            {
                SongListViewModel.InitialSongList(SongList);
            }
        }

        public void ChangePlayList()
        {
            if (IsSongListChanged)
            {
                IsSongListChanged = false;
                PlayList.Clear();

                //var tempList = new List<Song>();
                //SongList.ForEach(i => tempList.Add(i));
                PlayList = new List<Song>(SongList);
            }
        }

        public void ChangePlayingSong(Song song)
        {
            if (!Equals(song, PlayingSong))
            {
                PlayingSong = song;
            }
        }

        public void AddPlayList(Song song)
        {
            if (PlayList != null)
            {
                var tempList = new List<Song>(PlayList)
                {
                    song
                };
                PlayList = tempList;
            }
        }

        public void DeletePlayList(Song song)
        {
            if (PlayList != null)
            {
                var tempList = new List<Song>(PlayList);
                tempList.Remove(song);
                PlayList = tempList;
            }
        }
        public DelegateCommand<Button> SelectTagsCommand { get; set; }
        /// <summary>
        /// 确认选中的标签，在歌曲列表显示包含选中标签的歌曲
        /// </summary>
        private void OnSelectTags(Button button)
        {
            //Songs.SongList.Clear();
            TagButtonModel.Instance.SetTagModel(ref button);
            //MainViewModel.SongList = SongListModel.GetSelectedSongs(TagButtonModel.Instance.SongTags);
        }

        public TagsPanelViewModel TagsPanelViewModel { get; set; }
        public SongListViewModel SongListViewModel { get; set; }
        public ControlViewModel ControlViewModel { get; set; }
        public PlayListViewModel PlayListViewModel { get; set; }

        public MainViewModel()
        {
            SongListViewModel = new SongListViewModel(this);

            SongList = SongListModel.GetSongsDb();
            PlayList = new List<Song>();
            TagsPanelViewModel = new TagsPanelViewModel(this);
            ControlViewModel = new ControlViewModel(this);
            PlayListViewModel = new PlayListViewModel(this);

            SelectTagsCommand = new DelegateCommand<Button>(OnSelectTags);
        }
    }

    public class SongListItem
    {
        public Song Song { get; set; }
        public int SongNum { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
