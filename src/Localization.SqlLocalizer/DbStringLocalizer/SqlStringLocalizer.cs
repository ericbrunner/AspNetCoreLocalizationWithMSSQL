﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Localization.SqlLocalizer.DbStringLocalizer
{
    public class SqlStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _localizations;

        private readonly string _resourceKey;
        private bool _returnKeyOnlyIfNotFound;

        public SqlStringLocalizer(Dictionary<string, string> localizations, string resourceKey, bool returnKeyOnlyIfNotFound)
        {
            _localizations = localizations;
            _resourceKey = resourceKey;
            _returnKeyOnlyIfNotFound = returnKeyOnlyIfNotFound;
        }
        public LocalizedString this[string name]
        {
            get
            {
                bool notSucceed;
                var text = GetText(name, out notSucceed);
                
                return new LocalizedString(name, text,notSucceed);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return this[name];
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
            
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetText(string key,out bool notSucceed)
        {

#if NET451
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
#elif NET46
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
#else
            var culture = CultureInfo.CurrentCulture.ToString();
#endif
            string computedKey = $"{key}.{culture}";

            string result;
            if (_localizations.TryGetValue(computedKey, out result))
            {
                notSucceed = false;
                return result;
            }
            else
            {
                notSucceed = true;
                if(_returnKeyOnlyIfNotFound)
                {
                    return key;
                }

                return _resourceKey + "." + computedKey;
            }
        }
    }
}
