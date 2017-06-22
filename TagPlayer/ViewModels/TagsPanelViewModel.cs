using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TagPlayer.Model;
using System.Windows.Input;
using Prism.Commands;
using System.Windows.Controls;

namespace TagPlayer.ViewModels
{
    public class TagsPanelViewModel : BindableBase
    {
        public MainViewModel MainViewModel { get; set; }
        //public SongListModel SongListModel { get; set; } = new SongListModel();

        public ICommand LoadSongListCommand { get; set; }

        private void OnLoadSongList()
        {
            MainViewModel.SongList = SongListModel.LoadDirectorySongList();
        }

        public ICommand SureCommand { get; set; }
        private void OnSure()
        {
            MainViewModel.SongList = SongListModel.GetSelectedSongs(TagButtonModel.Instance.SongTags);
        }

        public ICommand PlaySongCommand { get; set; }

        private void OnPlaySong()
        {
            if (MainViewModel.SongList.Any())
            {
                var selectedSong = MainViewModel.SongList.FirstOrDefault();
                MainViewModel.ChangePlayingSong(selectedSong);
                MainViewModel.ChangePlayList();
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

        public TagsPanelViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            LoadSongListCommand = new DelegateCommand(OnLoadSongList);
            SureCommand = new DelegateCommand(OnSure);
            PlaySongCommand = new DelegateCommand(OnPlaySong);
            SelectTagsCommand = new DelegateCommand<Button>(OnSelectTags);
        }
    }
}
