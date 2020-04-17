using System.Linq;
using System.Text;
using AN.Integration.Database.Models.Models;

namespace AN.Integration.Database.Query
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

        public string GetInsertQuery(IDatabaseTable value)
        {
            var props = value.GetType().GetProperties();
            var columns = string.Join(",", props.Select(p => $"[{p.Name}]"));
            _stringBuilder.Clear();
            _stringBuilder.Append($"insert into [{value.GetType().Name}]\n");
            _stringBuilder.Append($"({columns})\nvalues\n(");

            foreach (var prop in props)
            {
                _stringBuilder.Append($"{prop.GetValue(value)},");
            }

            _stringBuilder.Replace(',', ')', _stringBuilder.Length - 1, 1);
            return _stringBuilder.ToString().ToLower();
        }

        public string GetUpdateQuery(IDatabaseTable value)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append($"update [{value.GetType().Name}]\nset");

            foreach (var prop in value.GetType().GetProperties())
            {
                _stringBuilder.Append($"[{prop.Name}] = {prop.GetValue(value)},");
            }

            _stringBuilder.Remove(_stringBuilder.Length - 1, 1);
            _stringBuilder.Append($"\nwhere [id] = {value.Id}");

            return _stringBuilder.ToString().ToLower();
        }

        public string GetDeleteQuery(IDatabaseTable value)
        {
            _stringBuilder.Clear();
            _stringBuilder.Append($"delete from [{value.GetType().Name}]" +
                                  $"\nwhere [id]={value.Id}");

            return _stringBuilder.ToString().ToLower();
        }

        private string GetCondition(QueryFilter filter)
        {
            _stringBuilder.Clear();
            filter.QueryConditions.ToList().ForEach(i =>
            {
                if (_stringBuilder.Length != 0) _stringBuilder.Append($" {filter.Type} ");
                _stringBuilder.Append($"{i.Column} {i.Operator} {i.Value}");
            });
            return $" where {_stringBuilder}".ToLower();
        }
    }
}