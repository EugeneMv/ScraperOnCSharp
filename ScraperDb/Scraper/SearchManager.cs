using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScraperDb.Scraper
{
    public class SearchManager
    {
        public static string SearchToUrl(SearchCriteria search)
        {
            return "https://www.booking.com/searchresults.en-gb.html?" +
                    "&tmpl=searchresults" +
                    "&checkin_month=" + search.CheckIn.Month +
                    "&checkin_monthday=" + search.CheckIn.Day +
                    "&checkin_year=" + search.CheckIn.Year +
                    "&checkout_month=" + search.CheckOut.Month +
                    "&checkout_monthday=" + search.CheckOut.Day +
                    "&checkout_year=" + search.CheckOut.Year +
                    "&dest_type=district&group_children=0&no_rooms=1&raw_dest_type=city&room1=A%2CA&sb_price_type=total" +
                    "&ss=" + search.Destination +
                    //          "&top_ufis=1&selected_currency=USD" +
                    "&rows=25" +
                    "&offset=" + search.Offset;
        }
    }
}
