using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
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
        //public SongListModel SongListModel { get; set; } = new SongListModel();

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

        public DelegateCommand<ListBox> DoubleClickCommand { get; set; }
        private void OnDoubleClick(Selector listBox)
        {
            if (listBox.SelectedItem is SongListItem songListItem)
            {
                var selectedSong = songListItem.Song;
                MainViewModel.ChangePlayingSong(selectedSong);
                MainViewModel.ChangePlayList();
            }
        }

        public DelegateCommand<ListBox> PlayMenuCommand { get; set; }
        public DelegateCommand<ListBox> EditCommand { get; set; }
        private void OnEdit(Selector listBox)
        {
            var songListItem = listBox.SelectedItem as SongListItem;
            if (songListItem == null) return;
            var selectedSong = songListItem.Song;

            //var tagEditViewModel = new TagEditViewModel(selectedSong);
            var tagsEditViewModel = new TagsEditViewModel(selectedSong);
            var tagEditWindow = new TagsEditingWindow()
            {
                DataContext = tagsEditViewModel
            };
            tagEditWindow.ShowDialog();

            if (tagEditWindow.DialogResult != true) return;
            //SongListModel.ClearSongTags(songListItem.Song);//将数据库中该歌曲的标签清空
            //selectedSong.Tags = new List<string>(tagsEditViewModel.SongTags);
            //SongListModel.SaveSongTags(songListItem.Song);
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

        public SongListViewModel(MainViewModel mainViewModel)
        {
            IsShow = false;
            MainViewModel = mainViewModel;
            DisSongList = new List<SongListItem>();

            DoubleClickCommand = new DelegateCommand<ListBox>(OnDoubleClick);
            PlayMenuCommand = new DelegateCommand<ListBox>(OnDoubleClick);
            EditCommand = new DelegateCommand<ListBox>(OnEdit);
            AddCommand = new DelegateCommand<ListBox>(OnAdd);
        }
    }
}
