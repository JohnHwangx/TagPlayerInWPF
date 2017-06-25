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

        public ICommand LoadSongListCommand { get; set; }

        private void OnLoadSongList()
        {
            MainViewModel.SongList = SongListModel.LoadDirectorySongList();
        }

        public ICommand SureCommand { get; set; }
        private void OnSure()
        {
            MainViewModel.SongList = SongListModel.GetSelectedSongs(TagButtonModel.Instance.SelectTags);
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

        public TagsPanelViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
            LoadSongListCommand = new DelegateCommand(OnLoadSongList);
            SureCommand = new DelegateCommand(OnSure);
            PlaySongCommand = new DelegateCommand(OnPlaySong);
        }
    }
}
