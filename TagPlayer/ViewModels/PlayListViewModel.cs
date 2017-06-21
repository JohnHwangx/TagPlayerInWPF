﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.Windows.Input;
using Prism.Commands;
using TagPlayer.Model;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TagPlayer.ViewModels
{
    public class PlayListViewModel : BindableBase
    {
        /// <summary>
        /// 为了改变PlayingSong而加入
        /// </summary>
        public MainViewModel MainViewModel { get; set; }
        //public SongListModel SongListModel { get; set; } = new SongListModel();
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
            DisPlayList = SongListModel.InitialSongList(songList);
        }

        public ICommand ShowPlayListCommand { get; set; }

        private void OnShowPlayList()
        {
            IsShowPlayList = !IsShowPlayList;
        }

        public DelegateCommand<ListBox> DoubleClickCommand { get; set; }
        private void OnDoubleClick(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                MainViewModel.ChangePlayingSong(selectedSong);
            }
        }

        public DelegateCommand<ListBox> EditCommand { get; set; }
        private void OnEdit(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                //todo
            }
        }

        public DelegateCommand<ListBox> DeleteCommand { get; set; }
        private void OnDelete(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                MainViewModel.DeletePlayList(selectedSong);
            }
        }

        public ICommand ClearCommand { get; set; }
        private void OnClear()
        {
            if (MainViewModel.PlayList != null && MainViewModel.PlayList.Any())
            {
                MainViewModel.PlayList.Clear();
            }
        }


        public PlayListViewModel(MainViewModel mainViewModel)
        {
            IsShowPlayList = false;
            MainViewModel = mainViewModel;
            ShowPlayListCommand = new DelegateCommand(OnShowPlayList);
            DoubleClickCommand = new DelegateCommand<ListBox>(OnDoubleClick);
            EditCommand = new DelegateCommand<ListBox>(OnEdit);
            DeleteCommand = new DelegateCommand<ListBox>(OnDelete);
            ClearCommand = new DelegateCommand(OnClear);
        }
    }
}
