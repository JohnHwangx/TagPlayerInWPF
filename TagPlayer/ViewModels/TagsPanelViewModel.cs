using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using TagPlayer.Model;
using System.Windows.Input;
using Prism.Commands;

namespace TagPlayer.ViewModels
{
    public class TagsPanelViewModel:BindableBase
    {
        public MainViewModel MainViewModel { get; set; }
        public SongListModel SongListModel { get; set; } = new SongListModel();

        public ICommand LoadSongListCommand { get; set; }

        private void OnLoadSongList()
        {
            MainViewModel.SongList=SongListModel.LoadDirectorySongList();
        }

        public TagsPanelViewModel(MainViewModel mainViewModel)
        {
            MainViewModel=mainViewModel;
            LoadSongListCommand = new DelegateCommand(OnLoadSongList);
        }
    }
}
