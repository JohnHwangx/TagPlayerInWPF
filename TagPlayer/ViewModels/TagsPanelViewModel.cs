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

        private List<string> paths;

        private void OnLoadSongList()
        {
            paths = SongListOperator.Instance.LoadDirectorySongList();

            var progressable = new LoadProgressBar(this, p =>
            {
                if (!p.IsCanceled)
                {
                    MessageBox.Show("导入完成");
                }
            });
            ProgressRunner.Run(progressable, new List<string> { "导入歌曲", "保存歌曲" });
        }

        public void DoWithProgressable(Action<int, int, string> progress)
        {
            MainViewModel.SongList.Clear();
            int current = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                current++;
                Song song = new Song(paths[i]);
                MainViewModel.SongList.Add(song);
                if (i % 20 == 0)
                {
                    MainViewModel.SongListViewModel.InitialSongList(MainViewModel.SongList);
                    //Thread.Sleep(1000);
                }
                progress(0, current * 100 / paths.Count, $"正在导入歌曲：{song.Title}");
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
