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
using System.Threading;

namespace TagPlayer.ViewModels
{
    public class TagsPanelViewModel : BindableBase
    {
        public MainViewModel MainViewModel { get; set; }

        public ICommand LoadSongListCommand { get; set; }

        private void OnLoadSongList()
        {
            var paths = SongListOperator.Instance.LoadDirectorySongList();
            MainViewModel.SongList.Clear();
            for (int i = 0; i < paths.Count; i++)
            {
                Song song = new Song(paths[i]);
                MainViewModel.SongList.Add(song);
                if (i % 20 == 0)
                {
                    MainViewModel.SongListViewModel.InitialSongList(MainViewModel.SongList);
                    //Thread.Sleep(1000);
                }
            }
        }

        public ICommand SureCommand { get; set; }
        private void OnSure()
        {
            MainViewModel.SongList = SongListModel.Instance.GetSelectedSongs(TagButtonModel.Instance.SelectTags);
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
