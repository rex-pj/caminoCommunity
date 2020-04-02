using HotChocolate.Language;
using HotChocolate.Types;
using System;

namespace Coco.Api.Framework.GraphQLTypes.Redefines
{
    public class DynamicType : ScalarType
    {
        public DynamicType() : base("Dynamic") { }

        public override Type ClrType => throw new NotImplementedException();

        public override bool IsInstanceOfType(IValueNode literal)
        {
            throw new NotImplementedException();
        }

        public override object ParseLiteral(IValueNode literal)
        {
            //if (literal is DateTimeValue timeValue)
            //{
            //    return timeValue.Value;
            //}

            //if (value is StringValue stringValue)
            //{
            //    return ParseValue(stringValue.Value);
            //}

            return null;
        }

        public override IValueNode ParseValue(object value)
        {
            //bool canParsed = DateTime.TryParse(value.ToString(), out _);
            //if (canParsed)
            //{
            //    //value, typeof(DateTime);
            //    return new StringValueNode
            //}
            //else
            //{
            //    return value;
            //}

            return null;
        }

        public override object Serialize(object value)
        {
            return ParseValue(value);
        }

        public override bool TryDeserialize(object serialized, out object value)
        {
            throw new NotImplementedException();
        }
    }
}
