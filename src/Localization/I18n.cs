using System;
using System.Collections.Generic;

namespace LoLFirewallManager.Localization
{
    public static class I18n
    {
        private static ILanguage _current;
        private static readonly Dictionary<string, ILanguage> _languages = new Dictionary<string, ILanguage>(StringComparer.OrdinalIgnoreCase);

        public static event Action LanguageChanged;

        static I18n()
        {
            Register(new EnglishLanguage());
            Register(new SpanishLanguage());
            Register(new PortugueseLanguage());
            Register(new ChineseLanguage());
            Register(new KoreanLanguage());
            Register(new JapaneseLanguage());

            _current = _languages["en"];
        }

        private static void Register(ILanguage lang)
        {
            _languages[lang.Code] = lang;
        }

        public static IEnumerable<ILanguage> SupportedLanguages
        {
            get { return _languages.Values; }
        }

        public static ILanguage Current
        {
            get { return _current; }
            set
            {
                if (value != null && _current != value)
                {
                    _current = value;
                    if (LanguageChanged != null)
                        LanguageChanged();
                }
            }
        }

        public static void SetLanguageByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return;
            ILanguage lang;
            if (_languages.TryGetValue(code, out lang))
            {
                Current = lang;
            }
        }

        public static ILanguage GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return _languages["en"];
            ILanguage lang;
            if (_languages.TryGetValue(code, out lang))
                return lang;
            return _languages["en"];
        }
    }
}

