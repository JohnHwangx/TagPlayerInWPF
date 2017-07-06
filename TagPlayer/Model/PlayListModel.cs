using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    public class PlayListModel:DbOperator
    {
        private const string DbName = "PlayerDb";
        private const string TableName = "PlayList";
        /// <summary>
        /// 将歌曲存入数据库
        /// </summary>
        public void SaveSongsDb(List<Song> songList)
        {
            if (songList == null || !songList.Any())
            {
                return;
            }
            if (!IsExistDb(DbName))//数据库不存在
            {
                CreateDb(DbName);//创建数据库
            }
            if (!IsExistTable(TableName))//表不存在
            {
                string createTableSql = @"path nvarchar(400) primary key";
                CreateTable(DbName, TableName, createTableSql);//创建数据表
            }
            else
            {
                ClearTable(DbName, TableName);
            }
            var columnSql = @"path";
            foreach (var song in songList)
            {
                var insertSql = "'" + EscConvertor(song.Path)+ "'";
                InsertTable(DbName, TableName, columnSql, insertSql);
            }
        }
        /// <summary>
        /// 读取数据库歌曲到歌曲列表
        /// </summary>
        public List<Song> GetSongsDb(List<Song> songs)
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
                //songList.Add(new Song
                //{
                //    Path = dataReader[0].ToString().Trim(),
                //    Title = dataReader[1].ToString().Trim(),
                //    Artist = dataReader[2].ToString().Trim(),
                //    Album = dataReader[3].ToString().Trim(),
                //    Duration = dataReader[4].ToString().Trim(),
                //});

                var Path = dataReader[0].ToString().Trim();
                //if (songs.Select(i => i.Path).Contains(Path))
                {
                    var song = songs.Where(i => i.Path == Path).FirstOrDefault();
                    songList.Add(song);
                }
            }
            dataReader.Dispose();
            dataReader.Close();

            return songList;
        }
    }
}
