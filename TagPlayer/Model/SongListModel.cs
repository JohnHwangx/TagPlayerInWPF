using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TagPlayer.Model
{
    /// <summary>
    /// 歌曲加载及显示
    /// </summary>
    public class SongListModel
    {
        //private List<Song> songList = new List<Song>();
        private void LoadSongs(string songListPath, List<Song> songList)
        {
            if (!Directory.Exists(songListPath)) return;
            foreach (var path in Directory.GetFileSystemEntries(songListPath))
            {
                if (File.Exists(path) &&
                    (File.GetAttributes(path) & FileAttributes.Hidden) != FileAttributes.Hidden &&
                    Path.GetExtension(path) == ".mp3")
                {
                    Song song = new Song(path);
                    songList.Add(song);
                }
                else if (Directory.Exists(path) &&
                    (new DirectoryInfo(path).Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                {
                    LoadSongs(path,songList);
                }
            }
        }

        /// <summary>
        /// 从文件夹加载歌曲列表
        /// </summary>
        /// <param name="songListPath">目录路径</param>
        /// <returns>歌曲列表</returns>
        public List<Song> LoadDirectorySongList()
        {
            List<Song> songList = new List<Song>();
            using (var dirChooser = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dirChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadSongs(dirChooser.SelectedPath ,songList);
                }
            }
            return songList;
        }

        public List<SongListItem> InitialSongList(List<Song> songs)
        {
            var songList = new List<SongListItem>();
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
    }
}
