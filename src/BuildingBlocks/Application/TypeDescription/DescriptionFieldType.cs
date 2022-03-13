using System.Collections.Generic;

namespace HelpLine.BuildingBlocks.Application.TypeDescription
{
    public abstract class DescriptionFieldType
    {

    }

    public class PrimitiveDescriptionFieldType : DescriptionFieldType
    {
        public enum Primitives
        {
            Number,
            Boolean,
            String,
            Date
        }

        public Primitives Type { get; }

        public PrimitiveDescriptionFieldType(Primitives type)
        {
            Type = type;
        }
    }

    public class ArrayDescriptionFieldType : DescriptionFieldType
    {
        public DescriptionFieldType ItemType { get; }

        public ArrayDescriptionFieldType(DescriptionFieldType itemType)
        {
            ItemType = itemType;
        }
    }

    public class DictionaryDescriptionFieldType : DescriptionFieldType
    {
        public DescriptionFieldType KeyType { get; }
        public DescriptionFieldType ItemType { get; }

        public DictionaryDescriptionFieldType(DescriptionFieldType keyType, DescriptionFieldType itemType)
        {
            KeyType = keyType;
            ItemType = itemType;
        }
    }

    public class ClassDescriptionFieldType : DescriptionFieldType
    {
        public string Type { get; }

        public ClassDescriptionFieldType(string type)
        {
            Type = type;
        }
    }

    public class EnumDescriptionFieldType : DescriptionFieldType
    {
        public string Enum { get; }

        public EnumDescriptionFieldType(string @enum)
        {
            Enum = @enum;
        }
    }
}
