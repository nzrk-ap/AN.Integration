using System;
using System.Collections.Generic;
using static AN.Integration.Database.Query.QueryFilterElement;

namespace AN.Integration.Database.Query
{
    public class ConditionValueBuilder
    {
        public string GetValue(object value, OperatorType operatorType)
        {
            var textValue = value switch
            {
                string text => GetValueByOperator(text, operatorType),
                int number => number.ToString(),
                Guid guid => guid.ToString(),
                IEnumerable<string> textCollection => GetValueByOperator(textCollection, operatorType),
                IEnumerable<int> numberCollection => GetValueByOperator(numberCollection, operatorType),
                IEnumerable<Guid> guidCollection => GetValueByOperator(guidCollection, operatorType),
                _ => throw new ArgumentException($"Argument with type {value.GetType().Name} is not supported"),
            };
            return textValue;
        }

        private static string GetValueByOperator<T>(IEnumerable<T> values, OperatorType operatorType) =>
            (operatorType == OperatorType.In) ? $"({string.Join(",", values)})" :
            throw new ArgumentException($"Operator {operatorType} is not allowed for multiple values");

        private static string GetValueByOperator(string value, OperatorType operatorType) =>
            operatorType == OperatorType.Like ? $"'%{value}%'" : value;
    }
}
