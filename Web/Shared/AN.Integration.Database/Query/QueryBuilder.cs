using AN.Integration.Database.Models;
using System.Linq;
using System.Text;
using AN.Integration.Database.Query;

namespace AN.Integration.Database
{
    public class QueryBuilder
    {
        private StringBuilder _stringBuilder = new StringBuilder();

        public string GetQuery<T>(QueryFilter filter,
           params string[] columns) where T : IDatabaseTable
        {
            var columnString = columns.Any() ? string.Join(",", columns) : "*";
            return $"select {columnString} from {typeof(T).Name + 's'} {GetCondition(filter)}".ToLower();
        }

        private string GetCondition(QueryFilter filter)
        {
            _stringBuilder.Clear();
            filter.QueryConditions.ToList().ForEach(i =>
                {
                    if (_stringBuilder.Length != 0) _stringBuilder.Append($" {filter.Type} ");
                    _stringBuilder.Append($"{i.Column} {i.Operator} {i.Value}");
                });
            return $" where {_stringBuilder.ToString()}".ToLower();
        }
    }
}
