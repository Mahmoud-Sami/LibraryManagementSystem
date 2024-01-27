using MySql.Data.MySqlClient;

namespace LMS.DataAccess.Helpers
{
    public static class MySqlCommandExtensions
    {
        public static MySqlParameter[] AddArrayParameters<T>(this MySqlCommand cmd, string paramNameRoot, IEnumerable<T> values, MySqlDbType? dbType = null, int? size = null)
        {
            var parameters = new List<MySqlParameter>();
            var parameterNames = new List<string>();
            var paramNbr = 1;
            foreach (var value in values)
            {
                var paramName = string.Format("@{0}{1}", paramNameRoot, paramNbr++);
                parameterNames.Add(paramName);
                MySqlParameter p = new MySqlParameter(paramName, value);
                if (dbType.HasValue)
                    p.MySqlDbType = dbType.Value;
                if (size.HasValue)
                    p.Size = size.Value;
                cmd.Parameters.Add(p);
                parameters.Add(p);
            }

            cmd.CommandText = cmd.CommandText.Replace("{" + paramNameRoot + "}", string.Join(",", parameterNames));

            return parameters.ToArray();
        }
    }
}
