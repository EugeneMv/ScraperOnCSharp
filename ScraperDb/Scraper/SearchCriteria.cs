using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDb.Scraper
{
    public class SearchCriteria : ICloneable
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Destination { get; set; }
        public int Offset { get; set; }

        public object Clone()
        {
            return new SearchCriteria()
            {
                CheckIn = CheckIn,
                CheckOut = CheckOut,
                Destination = Destination
            };
        }
    }
}
