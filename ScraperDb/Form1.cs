using ScraperDb.Model;
using ScraperDb.Scraper;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ScraperDb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var dateStart = DateTime.Now.AddDays(7);
            var dateEnd = dateStart.AddYears(1);

            label2.Text = dateStart.ToString("dd/MM/yyyy") + " - "+ dateEnd.ToString("dd/MM/yyyy");
            listBox1.SelectedIndex = DateTime.Now.Month-1;
            InitPages();

            tabPage2.Text = "Rating & Price";
            tabPage3.Text = "Rating & Amount";
            tabPage4.Text = "Price & Month";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Model1 m = new Model1();
            m.Database.ExecuteSqlCommand("delete from dbo.HotelHeaders");
            m.Database.ExecuteSqlCommand("delete from dbo.HotelPrices");

            var scrap = new ScraperProcessor();

            var cw = 52;

            for (int i = 0; i < cw; i++)
            {
                var date = DateTime.Now;
                date = date.AddDays(i * 7);

                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate (object state)
                {
                    scrap.Start(new SearchCriteria()
                    {
                        CheckIn = date,
                        CheckOut = date.AddDays(7),
                        Destination = "Minsk"
                    });
                }), null);
            }

            UpdProgress(scrap, 52);

            m.SaveChanges();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private async void UpdProgress(ScraperProcessor scraper, int i2)
        {
            progressBar1.Maximum = i2;
            await Task.Run(() => Tick(scraper, i2));
        }

        private void Tick(ScraperProcessor scraper, int i2)
        {
            int old = 0;
            do
            {
                if (old != scraper.Amount)
                {
                    BeginInvoke(new Action(() => { progressBar1.Value = scraper.Amount; }));
                    old = scraper.Amount;
                }
            } while (i2 > scraper.Amount);

            BeginInvoke(new Action(() => { Convert(scraper); progressBar1.Value = 0; }));
        }

        private void Convert(ScraperProcessor scraper)
        {
            var hotelHeaders = new List<HotelHeader>();

            foreach (var el in scraper.hotelData)
            {
                var hotel = hotelHeaders.FirstOrDefault(p => p.Name.Equals(el.Name));

                if (hotel == null)
                {
                    hotel = new HotelHeader()
                    {
                        Name = el.Name,
                        Rate = el.Rate
                    };

                    hotelHeaders.Add(hotel);
                }

                hotel.Prices.Add(new HotelPrice()
                {
                    Price = el.Price,
                    CheckDate = el.CheckIn
                });
            }

            Model1 model1 = new Model1();
            model1.HotelHeaders.AddRange(hotelHeaders);
            model1.SaveChanges();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void InitPages()
        {
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();

            Model1 model1 = new Model1();

            var headers = model1.HotelHeaders.ToList();
            var prices = model1.HotelPrices.ToList();
            var prc = prices.OrderBy(p => p.CheckDate).ToList();
            var date = DateTime.Now;

            for (int i = 1; i <= 12; i++, date = date.AddMonths(1))
            {
                chart5.Series[0].Points.AddXY(date.ToString("MMM"), prc.Where(p => p.CheckDate.Month == date.Month).Average(p => p.Price));
            }

            headers = headers.OrderBy(p => p.Rate).ToList();

            var list1 = model1.HotelHeaders.Where(p => p.Rate < 7);
            var list2 = model1.HotelHeaders.Where(p => p.Rate <= 8 && p.Rate >= 7);
            var list3 = model1.HotelHeaders.Where(p => p.Rate <= 9 && p.Rate > 8);
            var list4 = model1.HotelHeaders.Where(p => p.Rate <= 10 && p.Rate > 9);

            chart4.Series[0].Points.AddXY("< 7", list1.Count());
            chart4.Series[0].Points.AddXY("7 - 8", list2.Count());
            chart4.Series[0].Points.AddXY("8 - 9", list3.Count());
            chart4.Series[0].Points.AddXY("9 - 10", list4.Count());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            chart4.Series[0].Points.Clear();
            chart5.Series[0].Points.Clear();
            Model1 model1 = new Model1();

            var headers = model1.HotelHeaders.ToList();
            var prices = model1.HotelPrices.ToList();

            var months = prices.Select(p => p.CheckDate).Distinct();
            var prc = prices.OrderBy(p => p.CheckDate).ToList();

            var list = new List<DataPoint>();
            var date = DateTime.Now;

            for(int i = 1; i <= 12; i++, date=date.AddMonths(1))
            {
                chart5.Series[0].Points.AddXY(date.ToString("MMM"), prc.Where(p => p.CheckDate.Month == date.Month).Average(p => p.Price));
            }

            headers = headers.OrderBy(p => p.Rate).ToList();

            var list1 = model1.HotelHeaders.Where(p => p.Rate < 7);
            var list2 = model1.HotelHeaders.Where(p => p.Rate <= 8 && p.Rate >= 7);
            var list3 = model1.HotelHeaders.Where(p => p.Rate <= 9 && p.Rate > 8);
            var list4 = model1.HotelHeaders.Where(p => p.Rate <= 10 && p.Rate > 9);

            chart4.Series[0].Points.AddXY("< 7", list1.Count() );
            chart4.Series[0].Points.AddXY("7 - 8", list2.Count());
            chart4.Series[0].Points.AddXY("8 - 9", list3.Count());
            chart4.Series[0].Points.AddXY("9 - 10", list4.Count());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart3.Series[0].Points.Clear();
            int i = listBox1.SelectedIndex;
            i++;

            Model1 model1 = new Model1();

            if (model1.HotelHeaders.Count() == 0)
                return;

            var prices = model1.HotelPrices.ToList();

            var list1 = model1.HotelHeaders.Where(p => p.Rate < 7);
            var list2 = model1.HotelHeaders.Where(p => p.Rate <= 8 && p.Rate >= 7);
            var list3 = model1.HotelHeaders.Where(p => p.Rate <= 9 && p.Rate > 8);
            var list4 = model1.HotelHeaders.Where(p => p.Rate <= 10 && p.Rate > 9);

            if (list1.Where(p => p.Prices.Any(s => s.CheckDate.Month == i)).Count() != 0)
            {
                chart3.Series[0].Points.AddXY("< 7", list1.Average(p => p.Prices.Where(a => a.CheckDate.Month == i).Average(s => s.Price ?? 0)));
            }
            chart3.Series[0].Points.AddXY("7 - 8", list2.Average(p => p.Prices.Where(a => a.CheckDate.Month == i).Average(s => s.Price)));
            chart3.Series[0].Points.AddXY("8 - 9", list3.Average(p => p.Prices.Where(a => a.CheckDate.Month == i).Average(s => s.Price)));
            chart3.Series[0].Points.AddXY("9 - 10", list4.Average(p => p.Prices.Where(a => a.CheckDate.Month == i).Average(s => s.Price)));  
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
