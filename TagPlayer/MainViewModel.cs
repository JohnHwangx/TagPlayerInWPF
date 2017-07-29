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
using System.Windows;

namespace TagPlayer
{
    public class MainViewModel : BindableBase
    {
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

        private PlayState _playState = PlayState.暂停;

        public PlayState PlayState
        {
            get { return _playState; }
            set
            {
                _playState = value;
                //OnPlayStateChanged();
                RaisePropertyChanged("PlayState");
            }
        }

        private void OnPlayStateChanged()
        {
            switch (PlayState)
            {
                case PlayState.无文件:
                    break;
                case PlayState.无列表:
                    break;
                case PlayState.播放:
                    PlayModel.Instance.Play(PlayingSong.Path);
                    break;
                case PlayState.暂停:
                    PlayModel.Instance.Pause();
                    break;
                case PlayState.停止:
                    break;
                default:
                    break;
            }
        }

        public void LoadSongList()
        {
            var paths = SongListOperator.Instance.LoadDirectorySongList();
            SongList.Clear();
            for (int i = 0; i < paths.Count; i++)
            {
                Song song = new Song(paths[i]);
                SongList.Add(song);
                if (i % 10 == 0)
                {
                    SongListViewModel.InitialSongList(SongList);
                }
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

                PlayList = new List<Song>(SongList);
            }
        }

        public void ChangePlayList(List<Song> songs)
        {
            PlayList = new List<Song>(songs);
            //PlayListModel.Instance.SaveSongs(PlayList);
        }

        public void ChangePlayingSong(Song song)
        {
            if (!Equals(song, PlayingSong))
            {
                PlayingSong = song;
            }
        }

        public void AddPlayList(List<Song> songs)
        {
            if (PlayList != null)
            {
                var tempList = new List<Song>(PlayList);
                tempList.AddRange(songs);
                PlayList = tempList;
                //PlayListModel.Instance.SaveSongs(PlayList);
            }
        }

        public void DeleteAtSongList(List<Song> songs)
        {
            if (SongList != null)
            {
                var tempList = new List<Song>(SongList);
                foreach (var song in songs)
                {
                    tempList.Remove(song);
                }
                SongList = tempList;
                SongListModel.Instance.DeleteSongs(SongList);
            }
        }

        public DelegateCommand<Button> SelectTagsCommand { get; set; }
        /// <summary>
        /// 确认选中的标签，在歌曲列表显示包含选中标签的歌曲
        /// </summary>
        private void OnSelectTags(Button button)
        {
            TagButtonModel.Instance.SetTagModel(ref button, TagsType.SelectTags);
        }

        public DelegateCommand ClosingCommand { get; set; }
        private void OnClosing()
        {
            PlayListModel.Instance.SaveSongs(PlayList);
        }

        public TagsPanelViewModel TagsPanelViewModel { get; set; }
        public SongListViewModel SongListViewModel { get; set; }
        public ControlViewModel ControlViewModel { get; set; }
        public PlayListViewModel PlayListViewModel { get; set; }

        public MainViewModel()
        {
            SongListViewModel = new SongListViewModel(this);
            PlayListViewModel = new PlayListViewModel(this);
            TagsPanelViewModel = new TagsPanelViewModel(this);
            ControlViewModel = new ControlViewModel(this);

            SongList = SongListModel.Instance.GetSongsDb();
            PlayList = PlayListModel.Instance.LoadSongs(SongList);

            SelectTagsCommand = new DelegateCommand<Button>(OnSelectTags);
            ClosingCommand = new DelegateCommand(OnClosing);
        }
    }

    public class SongListItem
    {
        public Song Song { get; set; }
        public int SongNum { get; set; }
        public SolidColorBrush Color { get; set; }
    }
}
