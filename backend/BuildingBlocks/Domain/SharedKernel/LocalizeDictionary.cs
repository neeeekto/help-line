using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HelpLine.BuildingBlocks.Domain.SharedKernel
{
    public class LocalizeDictionary<T> : ReadOnlyDictionary<LanguageCode, T>
    {
        public LocalizeDictionary(IDictionary<LanguageCode, T> dictionary) : base(dictionary)
        {
        }
    }
}
