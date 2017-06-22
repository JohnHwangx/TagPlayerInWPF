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
    public class DbOperator
    {
        private const string StrConn = @"Data Source=..\..\Database\PlayerDb.db;Version=3;";
        private static readonly SQLiteConnection Conn = new SQLiteConnection(StrConn);

        public static bool IsExistDb(string dbName)
        {
            if (File.Exists($@"..\..\Database\{dbName}.db"))
            {
                return true;
            }
            return false;
        }

        public static bool IsExistTable(string tableName)
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

        public static void CreateDb(string dbName)
        {
            Directory.CreateDirectory($@"..\..\Database");
            SQLiteConnection.CreateFile($@"..\..\Database\{dbName}.db");
        }

        public static void CreateTable(string dbName, string tableName, string columnName)
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

        public static void InsertTable(string dbName, string tableName, string columnName, string parameter)
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

        public static SQLiteDataReader TableDataReader(string dbName, string select)
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

        public static string EscConvertor(string s)
        {
            return s.Replace("'", "''");
        }

        public static void ClearTable(string dbName, string tableName)
        {
            if (Conn.State != ConnectionState.Open)
            {
                Conn.Open();
            }
            //string clearSql = $"truncate table {tableName}";
            string clearSql = $"delete from {tableName}";
            var comm = new SQLiteCommand(clearSql, Conn);
            comm.ExecuteNonQuery();
            comm.Dispose();
        }

        public static void UpdateTable(string dbName, string tableName, string param, string path)
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
