using System.Collections.Generic;

namespace AN.Integration.Database.Query
{
    public class QueryFilter
    {
        public FilterType Type { get; set; }
        public IEnumerable<QueryFilterElement> QueryConditions { get; set; }

        public QueryFilter() { }

        public QueryFilter(FilterType filterType, IEnumerable<QueryFilterElement> queryConditions)
        {
            QueryConditions = queryConditions;
            Type = filterType;
        }

        public QueryFilter(IEnumerable<QueryFilterElement> queryConditions) :
            this(FilterType.Or, queryConditions)
        { }

        public enum FilterType
        {
            Or = 1,
            And
        }
    }
}
