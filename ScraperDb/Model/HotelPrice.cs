using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDb.Model
{
    public class HotelPrice
    {
        public int Id { get; set; }
        public int? Price { get; set; }
        public DateTime CheckDate { get; set; }
        public int? HotelHeaderId { get; set; }
    }
}
