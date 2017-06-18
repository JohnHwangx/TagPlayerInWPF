using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagPlayer.Model
{
    class DbOperator
    {
        private const string StrConn = @"Data Source=..\..\Database\PlayerDb.db;Version=3;";
        private static readonly SQLiteConnection Conn = new SQLiteConnection(StrConn);

        public bool IsExistDb(string dbName)
        {
            if (File.Exists($@"..\..\Database\{dbName}.db"))
            {
                return true;
            }
            return false;
        }
    }
}
