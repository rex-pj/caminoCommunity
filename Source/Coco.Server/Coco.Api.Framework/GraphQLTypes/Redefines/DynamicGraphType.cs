using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using System;

namespace Coco.Api.Framework.GraphQLTypes.Redefines
{
    public class DynamicGraphType : ScalarGraphType
    {
        public DynamicGraphType()
        {
            Name = "Dynamic";
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is DateTimeValue timeValue)
            {
                return timeValue.Value;
            }

            if (value is StringValue stringValue)
            {
                return ParseValue(stringValue.Value);
            }

            return null;
        }

        public override object Serialize(object value)
        {
            return ParseValue(value);
        }

        public override object ParseValue(object value)
        {
            bool canParsed = DateTime.TryParse(value.ToString(), out _);
            if (canParsed)
            {
                return ValueConverter.ConvertTo(value, typeof(DateTime));
            }
            else
            {
                return value;
            }
        }
    }
}
