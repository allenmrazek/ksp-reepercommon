using System;
using System.Linq;

namespace ReeperCommon.Serialization
{
    public class FieldSerializer : IConfigNodeItemSerializer
    {
        private readonly IConfigNodeItemSerializer _decorated;
        private readonly IGetObjectFields _fields;

        public FieldSerializer(IConfigNodeItemSerializer decorated, IGetObjectFields fields)
        {
            if (decorated == null) throw new ArgumentNullException("decorated");
            if (fields == null) throw new ArgumentNullException("fields");

            _decorated = decorated;
            _fields = fields;
        }


        public void Serialize(Type type, ref object target, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");

            if (target != null)
                foreach (var field in _fields.Get(target))
                {
                    var fieldSerializer = serializer.SerializerSelector.GetSerializer(field.FieldType);
                    if (!fieldSerializer.Any()) continue;

                    var value = field.GetValue(target);

                    fieldSerializer.Single().Serialize(field.FieldType, ref value, config, serializer);
                }

            _decorated.Serialize(type, ref target, config, serializer);
        }


        public void Deserialize(Type type, ref object target, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");

            // deserialize first, in case the target is null (or is going to be null)
            _decorated.Deserialize(type, ref target, config, serializer);

            if (target != null)
                foreach (var field in _fields.Get(target))
                {
                    var fieldSerializer = serializer.SerializerSelector.GetSerializer(field.FieldType);
                    if (!fieldSerializer.Any()) continue;

                    var value = field.GetValue(target);

                    fieldSerializer.Single().Deserialize(field.FieldType, ref value, config, serializer);
                    field.SetValue(target, value);
                }
        }
    }
}
