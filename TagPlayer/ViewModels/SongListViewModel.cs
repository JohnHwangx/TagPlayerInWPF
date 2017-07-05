using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TagPlayer.controls;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class SongListViewModel : BindableBase
    {
        /// <summary>
        /// 为了改变PlayList而加入
        /// </summary>
        public MainViewModel MainViewModel { get; set; }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                _isShow = value;
                RaisePropertyChanged("IsShow");
            }
        }


        private ObservableCollection<SongListItem> _disSongList;

        public ObservableCollection<SongListItem> DisSongList
        {
            get { return _disSongList; }
            set
            {
                _disSongList = value;
                RaisePropertyChanged("DisSongList");
            }
        }

        public void InitialSongList(List<Song> songList)
        {
            DisSongList = SongListModel.InitialSongList(songList);
        }

        public DelegateCommand<ListBox> DoubleClickCommand { get; set; }
        private void OnDoubleClick(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                MainViewModel.ChangePlayingSong(selectedSong);
                MainViewModel.ChangePlayList();

                MainViewModel.PlayState = PlayState.播放;
                PlayModel.Instance.Play(MainViewModel.PlayingSong.Path);
            }
        }

        public DelegateCommand<ListBox> PlayMenuCommand { get; set; }
        private void OnPlay(ListBox listBox)
        {
            var songListItems = listBox.SelectedItems;
            if (songListItems != null && songListItems.Count != 0)
            {
                var selectedSongs = new List<Song>();
                foreach (SongListItem songListItem in songListItems)
                {
                    selectedSongs.Add(songListItem.Song);
                }
                MainViewModel.ChangePlayList(selectedSongs);
                MainViewModel.ChangePlayingSong(selectedSongs[0]);

                MainViewModel.PlayState = PlayState.播放;
                PlayModel.Instance.Play(MainViewModel.PlayingSong.Path);
            }
        }

        public DelegateCommand<ListBox> EditCommand { get; set; }
        private void OnEdit(Selector listBox)
        {
            SongModel.EditSongTags(listBox);
        }
        public DelegateCommand<ListBox> AddCommand { get; set; }

        private void OnAdd(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                MainViewModel.AddPlayList(selectedSong);
            }
        }

        public DelegateCommand<ListBox> DeleteCommand { get; set; }

        private void OnDelete(ListBox listBox)
        {
            var songListItems = listBox.SelectedItems;
            if (songListItems!=null&& songListItems.Count!=0)
            {
                var selectedSongs = new List<Song>();
                foreach (SongListItem songListItem in songListItems)
                {
                    selectedSongs.Add(songListItem.Song);
                }
                MainViewModel.DeleteAtSongList(selectedSongs);
            }
        }

        public SongListViewModel(MainViewModel mainViewModel)
        {
            IsShow = false;
            MainViewModel = mainViewModel;
            DisSongList = new ObservableCollection<SongListItem>();

            DoubleClickCommand = new DelegateCommand<ListBox>(OnDoubleClick);
            PlayMenuCommand = new DelegateCommand<ListBox>(OnPlay);
            EditCommand = new DelegateCommand<ListBox>(OnEdit);
            AddCommand = new DelegateCommand<ListBox>(OnAdd);
            DeleteCommand = new DelegateCommand<ListBox>(OnDelete);
        }
    }
}
