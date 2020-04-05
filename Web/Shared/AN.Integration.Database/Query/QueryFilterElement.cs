using System;
using System.Collections.Generic;

namespace AN.Integration.Database
{
    public class QueryFilterElement
    {
        private readonly ConditionValueBuilder _conditionValueBuilder = new ConditionValueBuilder();
        private readonly IReadOnlyDictionary<OperatorType, string> _conditions = new Dictionary<OperatorType, string>
        { { OperatorType.Eq, "=" },
          { OperatorType.Like, "like" },
          { OperatorType.Neq, "<>" },
          { OperatorType.Greater, ">" },
          { OperatorType.GreaterOrEq, ">=" },
          { OperatorType.In, "in" },
          { OperatorType.Less, "<" },
          { OperatorType.LessOrEq, "<=" }
        };

        public string Column { get; }
        public string Operator { get; }
        public object Value { get; }

        public QueryFilterElement(string column, OperatorType operatorType, object value)
        {
            if (!_conditions.TryGetValue(operatorType, out var operatorValue)) throw new KeyNotFoundException();
            Column = column;
            Operator = operatorValue;
            Value = _conditionValueBuilder.GetValue(value, operatorType);
        }

        public enum OperatorType
        {
            Eq = 1,
            Neq,
            Like,
            In,
            Less,
            LessOrEq,
            Greater,
            GreaterOrEq
        }
    }
}
