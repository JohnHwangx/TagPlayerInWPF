using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        private SongListModel()
        {
            if (!IsExistDb(DbName))//数据库不存在
            {
                CreateDb(DbName);//创建数据库
            }
            if (!IsExistTable(TableName))//表不存在
            {
                string createTableSql = @"path nvarchar(400) primary key,title text,artist text,album text,duration text" +
                                   GetInsertSql(TagButtonModel.Instance.GetAllTags(), " bool default false");
                CreateTable(DbName, TableName, createTableSql);//创建数据表
            }
        }

        private static SongListModel _instance;

        public static SongListModel Instance
        {
            get { return _instance ?? (_instance = new SongListModel()); }
        }
        /// <summary>
        /// 将歌曲存入数据库
        /// </summary>
        public void SaveSongs(List<Song> songList)
        {
            if (songList==null||!songList.Any())
            {
                return;
            }

            DropTable(TableName);

            var columnSql = @"path,title,artist,album,duration";
            foreach (var song in songList)
            {
                var insertSql = $"'{ EscConvertor(song.Path)} ','{ EscConvertor(song.Title)}','{ EscConvertor(song.Artist)}','{ EscConvertor(song.Album) }','{song.Duration } '";
                InsertTable(DbName, TableName, columnSql, insertSql);
            }
        }

        internal void DeleteSongs(List<Song> songList)
        {
            if (songList == null || !songList.Any())
            {
                return;
            }
            var columnSql = "path";
            foreach (var song in songList)
            {
                var deleteSql = $"'Path={EscConvertor(song.Path)}'";
                DeleteTable(DbName, TableName, columnSql, deleteSql);
            }
        }
        private string GetInsertSql(List<string> list, string split)
        {
            return list.Aggregate("", (current, item) => current + ",[" + item + "]" + split);
        }
        /// <summary>
        /// 读取数据库歌曲到歌曲列表
        /// </summary>
        public List<Song> GetSongsDb()
        {
            var songList = new List<Song>();
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
        public void SaveSongTags(Song song)
        {
            if (song.Tags.Count == 0 || song.Tags == null) return;

            var updateSql = string.Join(",", song.Tags.Select(i => "[" + i + "]=1"));
            UpdateTable(DbName, TableName, updateSql, song.Path);
        }
        /// <summary>
        /// 清空数据库标签
        /// </summary>
        /// <param name="song"></param>
        public void ClearSongTags(Song song)
        {
            if (song.Tags.Count == 0 || song.Tags == null) return;

            var clearSql = string.Join(",", song.Tags.Select(i => "[" + i + "]=0"));
            UpdateTable(DbName, TableName, clearSql, song.Path);
            //song.Tags.Clear();
        }

        /// <summary>
        /// 获取满足标签的歌曲
        /// </summary>
        public List<Song> GetSelectedSongs(List<string> selectedTags)
        {
            var songList = new List<Song>();
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
