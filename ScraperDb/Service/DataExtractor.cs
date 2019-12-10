using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScraperDb.Service
{
    class DataExtractor
    {
        public static int AmountOfEntries(string s)
        {
            Regex regex = new Regex(@"([0-9]+\,?[0-9]*)", RegexOptions.IgnoreCase);
            var b = regex.Match(s);
            var res = int.Parse(b.Value, NumberStyles.AllowThousands);

            return res;
        }

        public static double HotelRate(string s)
        {
            Regex regex = new Regex(@"([0-9]+\.?[0-9]+)", RegexOptions.IgnoreCase);
            var b = regex.Match(s);
            var res = double.Parse(b.Value);

            return res;
        }

        public static int HotelPrice(string s)
        {
            Regex regex = new Regex(@"([0-9]+\,?[0-9]*)", RegexOptions.IgnoreCase);
            var b = regex.Match(s);
            var res = int.Parse(b.Value, NumberStyles.AllowThousands);

            return res;
        }
    }
}
