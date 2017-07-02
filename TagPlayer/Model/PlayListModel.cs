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
        private const string TableName = "SongList";
        /// <summary>
        /// 将歌曲存入数据库
        /// </summary>
        public static void SaveSongsDb(List<Song> songList)
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
    }
}
