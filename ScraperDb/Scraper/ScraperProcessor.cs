using HtmlAgilityPack;
using ScraperDb.Model;
using ScraperDb.Service;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ScraperDb.Scraper
{
    public class ScraperProcessor
    {
        public List<HotelHeader> hotelHeaders;

        private ScrapingBrowser browser;
        public List<HotelData> hotelData;
        private List<HtmlNode> htmlNodes;

        public int Amount = 0;

        private readonly string hotelBlock = "#hotellist_inner > div.sr_item";
        private readonly string hotelPrice = "div.bui-price-display__value";
        private readonly string hotelRate = "div.bui-review-score__badge";
        private readonly string hotelName = "span.sr-hotel__name";
        private readonly string entriesAmount = "#results_prev_next > p";

        public ScraperProcessor()
        {
            browser = new ScrapingBrowser();
            htmlNodes = new List<HtmlNode>();
            hotelData = new List<HotelData>();
            hotelHeaders = new List<HotelHeader>();
        }

        public void Start(SearchCriteria searchCriteria)
        {
            var url = SearchManager.SearchToUrl(searchCriteria);

            var count = GetDataCount(url);

            try
            {
                int o = (int)Math.Ceiling(((double)count) / 25);
                Parallel.For(0, o, (i) => Execute(i, searchCriteria));
            }
            catch (OutOfMemoryException)
            {
                return;
            }

            Amount++;
        }

        private void Execute(int i, SearchCriteria searchCriteria)
        {
            var clone = searchCriteria.Clone() as SearchCriteria;
            clone.Offset = 25 * i;
            var url = SearchManager.SearchToUrl(clone);

            var nodes = CollectData(url);

            //  htmlNodes.AddRange(nodes);
            foreach (var node in nodes)
            {
                var obj = ParseData(node);
                if (obj != null)
                {
                    hotelData.Add(obj);
                    obj.CheckIn = searchCriteria.CheckIn;
                }
            }
        }

        private int GetDataCount(string url)
        {
            var page = browser.NavigateToPage(new Uri(url));

            var s = page.Html.CssSelect(entriesAmount);
            int count = 0;
            try
            {
                count = DataExtractor.AmountOfEntries(s.FirstOrDefault().InnerText);          
            }
            catch (Exception)
            {
                return 0;
            }
            return count;
        }

        private HtmlNode[] CollectData(string url)
        {
            ScrapingBrowser browser1 = new ScrapingBrowser();

            var page = browser1.NavigateToPage(new Uri(url as string));
            var data = page.Html.CssSelect(hotelBlock).ToArray();

            return data;
        }

        private HotelData ParseData(HtmlNode htmlNode)
        {
            var hotelInfo = new HotelData();

            try
            {
                hotelInfo.Name = htmlNode.CssSelect(hotelName).FirstOrDefault().InnerText;
                hotelInfo.Rate = DataExtractor.HotelRate(htmlNode.CssSelect(hotelRate).FirstOrDefault().InnerText);
                hotelInfo.Price = DataExtractor.HotelPrice(htmlNode.CssSelect(hotelPrice).FirstOrDefault().InnerText);
            }
            catch (Exception)
            {
                return null;
            }

            return hotelInfo;
        }
    }
}
