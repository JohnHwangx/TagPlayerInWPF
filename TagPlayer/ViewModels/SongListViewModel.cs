using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TagPlayer.Model;

namespace TagPlayer.ViewModels
{
    public class SongListViewModel : BindableBase
    {
        public MainViewModel MainViewModel { get; set; }
        public SongListModel SongListModel { get; set; } = new SongListModel();

        private List<SongListItem> _disSongList;

        public List<SongListItem> DisSongList
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

        public DelegateCommand<ListBox> DoubleClickSongListCommand { get; set; }
        private void OnDoubleClickSongList(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                if (!Equals(selectedSong,MainViewModel.PlayingSong))
                {
                    MainViewModel.PlayingSong = selectedSong;
                }
            }

            if (MainViewModel.IsSongListChanged)
            {
                MainViewModel.IsSongListChanged = false;
                MainViewModel.PlayList.Clear();

                //var tempList = new List<Song>();
                //MainViewModel.SongList.ForEach(i => tempList.Add(i));
                MainViewModel.PlayList = new List<Song>(MainViewModel.SongList); 
            }
        }

        public SongListViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            DisSongList = new List<SongListItem>();

            DoubleClickSongListCommand = new DelegateCommand<ListBox>(OnDoubleClickSongList);
        }
    }
}
