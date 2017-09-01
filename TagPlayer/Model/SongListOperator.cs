using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace TagPlayer.Model
{
    public class SongListOperator : DispatcherObject
    {
        private SongListOperator()
        {
            SongList = new List<Song>();
        }

        private static SongListOperator _instance;
        public static SongListOperator Instance
        {
            get { return _instance ?? (_instance = new SongListOperator()); }
        }

        public List<Song> SongList { get; set; }

        /// <summary>
        /// 从文件夹加载歌曲列表
        /// </summary>
        /// <param name="songListPath">目录路径</param>
        /// <returns>歌曲列表</returns>
        public List<string> LoadDirectorySongList()
        {
            List<string> songList = new List<string>();
            using (var dirChooser = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dirChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadSongs(dirChooser.SelectedPath, songList);
                }
            }
            //SongListModel.Instance.SaveSongs(songList);
            //return songList;
            return songList;
        }

        private void LoadSongList()
        {
            var paths = LoadDirectorySongList();
            foreach (var path in paths)
            {
                Song song = new Song(path);
            }
        }

        private void LoadSongs(string songListPath, List<string> songPathList)
        {
            if (!Directory.Exists(songListPath)) return;
            foreach (var path in Directory.GetFileSystemEntries(songListPath))
            {
                if (File.Exists(path) &&
                    (File.GetAttributes(path) & FileAttributes.Hidden) != FileAttributes.Hidden &&
                    Path.GetExtension(path) == ".mp3")
                {
                    //Song song = new Song(path);
                    //Song song = new Song(path, path);
                    //Thread.Sleep(100);
                    songPathList.Add(path);
                }
                else if (Directory.Exists(path) &&
                    (new DirectoryInfo(path).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    LoadSongs(path, songPathList);
                }
            }
        }

        private delegate void TaskDelegate(string path, List<Song> songList);

        /// <summary>
        /// 将歌曲集合转化为歌曲列表
        /// </summary>
        /// <param name="songs"></param>
        /// <returns></returns>
        public ObservableCollection<SongListItem> InitialSongList(List<Song> songs)
        {
            var songList = new ObservableCollection<SongListItem>();
            for (int i = 0; i < songs.Count; i++)
            {
                songList.Add(new SongListItem
                {
                    Song = songs[i],
                    SongNum = i + 1,
                    Color = i % 2 == 1
                        ? new SolidColorBrush(Colors.White)
                        : new SolidColorBrush(Colors.AliceBlue)
                });
            }
            return songList;
        }

        public SongListItem InitialSong(Song song,int i)
        {
            return new SongListItem
            {
                Song = song,
                SongNum = i + 1,
                Color = i % 2 == 1
                    ? new SolidColorBrush(Colors.White)
                    : new SolidColorBrush(Colors.AliceBlue)
            };
        }
    }
}
