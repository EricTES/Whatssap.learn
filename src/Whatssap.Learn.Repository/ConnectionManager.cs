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

        public static char? GetCharFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetChar(reader.GetOrdinal(columnName));
        }

        public static int? GetIntFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt32(reader.GetOrdinal(columnName));
        }

        public static long? GetLongFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetInt64(reader.GetOrdinal(columnName));
        }

        public static double? GetDoubleFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetDouble(reader.GetOrdinal(columnName));
        }

        public static float? GetFloatFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetFloat(reader.GetOrdinal(columnName));
        }

        public static bool? GetBooleanFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetBoolean(reader.GetOrdinal(columnName));
        }

        public static DateTime? GetDateFromReader(this DbDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName))) return null;
            return reader.GetDateTime(reader.GetOrdinal(columnName));
        }
    }
}
