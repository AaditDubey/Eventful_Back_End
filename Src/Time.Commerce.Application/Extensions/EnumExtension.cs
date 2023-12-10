using System.ComponentModel;

namespace Time.Commerce.Application.Extensions
{
    public static class EnumExtension
    {
        public static List<KeyValuePair<int, string>> GetEnumValuesAndDescriptions<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            List<KeyValuePair<int, string>> enumValList = new List<KeyValuePair<int, string>>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add(new KeyValuePair<int, string>((int)e, (attributes.Length > 0) ? attributes[0].Description : e.ToString()));
            }

            return enumValList;
        }

        public static List<string> GetEnumValuesAsString<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            List<string> enumValList = new List<string>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add((attributes.Length > 0) ? attributes[0].Description : e.ToString());
            }

            return enumValList;
        }

        public static List<KeyValuePair<string, string>> GetEnumValuesAsStringWithDescription<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T is not System.Enum");

            List<KeyValuePair<string, string>> enumValList = new List<KeyValuePair<string, string>>();

            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var fi = e.GetType().GetField(e.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                enumValList.Add(new KeyValuePair<string, string>(e.ToString(), (attributes.Length > 0) ? attributes[0].Description : e.ToString()));
            }

            return enumValList;
        }
    }
}
