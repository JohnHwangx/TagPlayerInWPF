using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace TagPlayer.Model
{
    /// <summary>
    /// 歌曲加载及显示
    /// </summary>
    public class SongListModel : DbOperator
    {
        private const string DbName = "PlayerDb";
        private const string TableName = "SongList";
        //private List<Song> songList = new List<Song>();
        private static void LoadSongs(string songListPath, List<Song> songList)
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
                    LoadSongs(path, songList);
                }
            }
        }

        /// <summary>
        /// 从文件夹加载歌曲列表
        /// </summary>
        /// <param name="songListPath">目录路径</param>
        /// <returns>歌曲列表</returns>
        public static List<Song> LoadDirectorySongList()
        {
            List<Song> songList = new List<Song>();
            using (var dirChooser = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dirChooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadSongs(dirChooser.SelectedPath, songList);
                }
            }
            SaveSongsDb(songList);
            return songList;
        }
        /// <summary>
        /// 将歌曲集合转化为歌曲列表
        /// </summary>
        /// <param name="songs"></param>
        /// <returns></returns>
        public static List<SongListItem> InitialSongList(List<Song> songs)
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
        /// <summary>
        /// 将歌曲存入数据库
        /// </summary>
        public static void SaveSongsDb(List<Song> songList)
        {
            if (songList==null||!songList.Any())
            {
                return;
            }
            if (!IsExistDb(DbName))//数据库不存在
            {
                CreateDb(DbName);//创建数据库
            }
            if (!IsExistTable(TableName))//表不存在
            {
                string createTableSql = @"path nvarchar(400) primary key,title text,artist text,album text,duration text" +
                                   GetInsertSql(GetAllTags(), " bool default false");
                CreateTable(DbName, TableName, createTableSql);//创建数据表
            }
            else
            {
                ClearTable(DbName, TableName);
            }
            var columnSql = @"path,title,artist,album,duration";
            foreach (var song in songList)
            {
                var insertSql = "'" + EscConvertor(song.Path) + "','" + EscConvertor(song.Title) + "','" + EscConvertor(song.Artist) + "','" + EscConvertor(song.Album) + "','" +
                                song.Duration + "'";
                InsertTable(DbName, TableName, columnSql, insertSql);
            }
        }

        /// <summary>
        /// 从XML文件中获得所有标签
        /// </summary>
        /// <returns>标签集合</returns>
        public static List<string> GetAllTags()
        {
            var tagList = new List<string>();
            //string xmlFileName = Path.Combine(Environment.CurrentDirectory, @"Image\SongTags.xml");
            var path = @"..\..\Image\SongTags.xml";
            XDocument xDoc = XDocument.Load(path/*@"C:\Users\john\Source\Repos\Player4\Player4\Image\SongTags.xml"*/);
            var tags = xDoc.Descendants("Tags");
            foreach (var category in tags)
            {
                foreach (var tag in category.Elements())
                {
                    foreach (var xElement in tag.Elements())
                    {
                        tagList.Add(xElement.Value);
                    }
                }
            }
            //MessageBox.Show(tagList.Count.ToString());
            return tagList;
        }
        private static string GetInsertSql(List<string> list, string split)
        {
            return list.Aggregate("", (current, item) => current + ",[" + item + "]" + split);
        }
        /// <summary>
        /// 读取数据库歌曲到歌曲列表
        /// </summary>
        public static List<Song> GetSongsDb()
        {
            var songList = new List<Song>();
            if (!IsExistDb(DbName) || !IsExistTable(TableName))
            {
                return songList;
            }
            var selectSql = @"select * from " + TableName;
            var dataReader = TableDataReader(DbName, selectSql);
            while (dataReader.Read())
            {
                var tagList = new List<string>();
                for (int i = 4; i < dataReader.FieldCount; i++)
                {
                    if (dataReader[i].ToString().Equals("True"))
                    {
                        tagList.Add(dataReader.GetName(i));
                    }
                }
                songList.Add(new Song
                {
                    Path = dataReader[0].ToString().Trim(),
                    Title = dataReader[1].ToString().Trim(),
                    Artist = dataReader[2].ToString().Trim(),
                    Album = dataReader[3].ToString().Trim(),
                    Duration = dataReader[4].ToString().Trim(),
                    Tags = tagList
                });
            }
            dataReader.Dispose();
            dataReader.Close();

            return songList;
        }
        public static void SaveSongTags(Song song)
        {
            if (!IsExistDb(DbName) || !IsExistTable(TableName))
            {
                return;
            }
            if (song.Tags.Count == 0 || song.Tags == null) return;

            var updateSql = string.Join(",", song.Tags.Select(i => "[" + i + "]=1"));
            UpdateTable(DbName, TableName, updateSql, song.Path);
        }
        /// <summary>
        /// 清空数据库标签
        /// </summary>
        /// <param name="song"></param>
        public static void ClearSongTags(Song song)
        {
            if (!IsExistDb(DbName) || !IsExistTable(TableName))
            {
                return;
            }
            if (song.Tags.Count == 0 || song.Tags == null) return;

            var clearSql = string.Join(",", song.Tags.Select(i => "[" + i + "]=0"));
            UpdateTable(DbName, TableName, clearSql, song.Path);
            //song.Tags.Clear();
        }

        /// <summary>
        /// 获取满足标签的歌曲
        /// </summary>
        public static List<Song> GetSelectedSongs(List<string> selectedTags)
        {
            var songList = new List<Song>();
            if (!IsExistDb(DbName) || !IsExistTable(TableName))
            {
                return songList;
            }
            string selectSql;
            if (selectedTags.Count == 0 || selectedTags == null)
            {
                selectSql = $"select * from {TableName}";
            }
            else
            {
                selectSql = $"select * from {TableName} where {string.Join(" and ", selectedTags.Select(i => "[" + i + "]=1"))}";
            }
            var dataReader = TableDataReader(DbName, selectSql);
            while (dataReader.Read())
            {
                var tagList = new List<string>();
                for (int i = 5; i < dataReader.FieldCount; i++)
                {
                    if (dataReader[i].ToString().Equals("True"))
                    {
                        tagList.Add(dataReader.GetName(i));
                    }
                }
                songList.Add(new Song
                {
                    Path = dataReader[0].ToString().Trim(),
                    Title = dataReader[1].ToString().Trim(),
                    Artist = dataReader[2].ToString().Trim(),
                    Album = dataReader[3].ToString().Trim(),
                    Duration = dataReader[4].ToString().Trim(),
                    Tags = tagList
                });
            }
            dataReader.Dispose();
            return songList;
        }
    }
}
