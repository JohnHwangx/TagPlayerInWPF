using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    public class PlayListModel : DbOperator
    {
        private const string DbName = "PlayerDb";
        private const string TableName = "PlayList";

        private PlayListModel()
        {
            if (!IsExistDb(DbName))//数据库不存在
            {
                CreateDb(DbName);//创建数据库
            }
            if (!IsExistTable(TableName))//表不存在
            {
                string createTableSql = @"path nvarchar(400) primary key";
                CreateTable(DbName, TableName, createTableSql);//创建数据表
            }
        }

        private static PlayListModel _instance;

        public static PlayListModel Instance
        {
            get { return _instance ?? (_instance = new PlayListModel()); }
        }

        /// <summary>
        /// 将歌曲存入数据库
        /// </summary>
        public void SaveSongs(List<Song> songList)
        {
            DropTable(TableName);
            var columnSql = @"path";
            foreach (var song in songList)
            {
                var insertSql = "'" + EscConvertor(song.Path) + "'";
                InsertTable(DbName, TableName, columnSql, insertSql);
            }
        }

        /// <summary>
        /// 读数据库歌曲，从歌曲列表中获取播放列表
        /// </summary>
        /// <param name="songs">歌曲列表</param>
        /// <returns>播放列表</returns>
        public List<Song> LoadSongs(List<Song> songs)
        {
            var songList = new List<Song>();

            var selectSql = @"select * from " + TableName;
            var dataReader = TableDataReader(DbName, selectSql);
            while (dataReader.Read())
            {
                var Path = dataReader[0].ToString().Trim();
                var song = songs.Where(i => i.Path == Path).FirstOrDefault();
                songList.Add(song);
            }
            dataReader.Dispose();
            dataReader.Close();

            return songList;
        }

        public void DropTable()
        {
            DropTable(TableName);
        }

        public void Deleta(List<Song> songList)
        {
            var columnSql = @"path";
            foreach (var song in songList)
            {
                var deleteSql = $"Path={EscConvertor(song.Path)}";
                DeleteTable(DbName, TableName, columnSql, deleteSql);
            }
        }
    }
}
