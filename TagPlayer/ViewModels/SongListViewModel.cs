﻿using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            DisSongList = SongListOperator.Instance.InitialSongList(songList);
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
            var selectedSongs = GetSelectedSongs(listBox);
            MainViewModel.ChangePlayList(selectedSongs);
            MainViewModel.ChangePlayingSong(selectedSongs[0]);

            MainViewModel.PlayState = PlayState.播放;
            PlayModel.Instance.Play(MainViewModel.PlayingSong.Path);
        }

        public DelegateCommand<ListBox> EditCommand { get; set; }
        private void OnEdit(Selector listBox)
        {
            SongModel.EditSongTags(listBox);
        }
        public DelegateCommand<ListBox> AddCommand { get; set; }

        private void OnAdd(ListBox listBox)
        {
            var selectedSongs = GetSelectedSongs(listBox);
            MainViewModel.AddPlayList(selectedSongs);
        }

        public DelegateCommand<ListBox> DeleteCommand { get; set; }

        private void OnDelete(ListBox listBox)
        {
            var selectedSongs = GetSelectedSongs(listBox);
            MainViewModel.DeleteAtSongList(selectedSongs);
        }

        public DelegateCommand<ListBox> ClearCommand { get; set; }

        private void OnClear(ListBox listBox)
        {
            var selectedSongs = GetSelectedSongs(listBox, true);
            MainViewModel.DeleteAtSongList(selectedSongs);
        }

        private List<Song> GetSelectedSongs(ListBox listBox, bool isAll = false)
        {
            var selectedSongs = new List<Song>();
            var songListItems = listBox.SelectedItems;
            if (isAll)
            {
                songListItems = listBox.Items;
            }
            if (songListItems.Count != 0)
            {
                foreach (SongListItem songListItem in songListItems)
                {
                    selectedSongs.Add(songListItem.Song);
                }
            }
            return selectedSongs;
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
            ClearCommand = new DelegateCommand<ListBox>(OnClear);
        }
    }
}
