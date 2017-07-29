using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    public abstract class DbOperator
    {
        private const string StrConn = @"Data Source=..\..\Database\PlayerDb.db;Version=3;";
        private readonly SQLiteConnection Conn = new SQLiteConnection(StrConn);
        /// <summary> 判断数据库是否存在 </summary>
        protected bool IsExistDb(string dbName)
        {
            if (File.Exists($@"..\..\Database\{dbName}.db"))
            {
                return true;
            }
            return false;
        }
        /// <summary> 判断数据表是否存在 </summary>
        protected bool IsExistTable(string tableName)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string tableExistSql = $"SELECT * FROM sqlite_master where type='table' and name='{tableName}'";
            var comm = new SQLiteCommand(tableExistSql, Conn);
            var dataReader = comm.ExecuteReader();
            if (dataReader.Read())
            {
                dataReader.Dispose();
                comm.Dispose();
                return true;
            }
            dataReader.Dispose();
            comm.Dispose();
            return false;
        }
        /// <summary> 创建数据库 </summary>
        protected void CreateDb(string dbName)
        {
            Directory.CreateDirectory($@"..\..\Database");
            SQLiteConnection.CreateFile($@"..\..\Database\{dbName}.db");
        }
        /// <summary> 创建数据表 </summary>
        protected void CreateTable(string dbName, string tableName, string columnName)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string tableSql = $"create table {tableName} ({columnName})";
            var comm = new SQLiteCommand(tableSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        protected void InsertTable(string dbName, string tableName, string columnName, string parameter)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string tableSql = $"insert into {tableName} ({columnName}) " +
                              $"values({parameter})";
            var comm = new SQLiteCommand(tableSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        protected void DeleteTable(string dbName,string tableName,string columnName,string parameter)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            var tableSql = $"delete from {tableName} where {parameter}";
            var comm = new SQLiteCommand(tableSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        protected void DropTable(string tableName)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }

            var tableSql = $"delete from {tableName}";
            var comm = new SQLiteCommand(tableSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        protected SQLiteDataReader TableDataReader(string dbName, string select)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string selectSql = $"{select}";
            var comm = new SQLiteCommand(selectSql, Conn);
            var dataReader = comm.ExecuteReader();
            comm.Dispose();
            return dataReader;
        }

        protected static string EscConvertor(string s)
        {
            return s.Replace("'", "''");
        }

        //public void ClearTable(string dbName, string tableName)
        //{
        //    if (Conn.State != ConnectionState.Open)
        //    {
        //        Conn.Open();
        //    }
        //    //string clearSql = $"truncate table {tableName}";
        //    string clearSql = $"delete from {tableName}";
        //    var comm = new SQLiteCommand(clearSql, Conn);
        //    comm.ExecuteNonQuery();
        //    comm.Dispose();
        //}

        protected void UpdateTable(string dbName, string tableName, string param, string path)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            string updateSql = $"update {tableName} set {param} where path='{EscConvertor(path)}'";
            var comm = new SQLiteCommand(updateSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }
    }
}
