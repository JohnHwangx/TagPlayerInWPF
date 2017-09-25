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
using System.Windows.Forms;
using TagPlayer.ProgressBar;
using ProgressBar;

namespace TagPlayer.ViewModels
{
    public class TagsPanelViewModel : BindableBase
    {
        public MainViewModel MainViewModel { get; set; }

        public ICommand LoadSongListCommand { get; set; }

        private List<string> _paths;

        private void OnLoadSongList()
        {
            _paths = SongListOperator.Instance.LoadDirectorySongList();

            var progressable = new LoadProgressBar(this, p =>
            {
                if (!p.IsCanceled)
                {
                    var text = $"共导入{MainViewModel.SongList.Count}首歌曲";
                    MessageBox.Show(text);
                }
            });
            ProgressRunner.Run(progressable);
        }

        internal void Cancel()
        {
            IsCanceled = true;
        }

        public bool IsCanceled { get; private set; }

        public void DoWithProgressable(Action<int, int, string> progress)
        {
            MainViewModel.SongList.Clear();
            var current = 0;
            foreach (var path in _paths)
            {
                if (IsCanceled)
                {
                    break;
                }
                current++;
                Song song = new Song(path);
                MainViewModel.SongList.Add(song);

                MainViewModel.SongListViewModel.InitialSongList(MainViewModel.SongList);

                progress(0, current * 100 / _paths.Count, $"正在导入歌曲：{song.Title}");
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
