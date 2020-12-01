using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;

namespace Whatssap.Learn.Repository
{
    public static class ConnectionManager
    {
        public static SqlConnection GetConnection(String connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public static string GetStringFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetString(reader.GetOrdinal(columnName));
        }

        public static int? GetIntFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        //add more reader method for all the data types
    }
}
