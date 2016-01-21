using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Data_load_utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string[] tickers;
        public MainWindow()
        {
            InitializeComponent();
        }

        //upload file button will load csv into tickers 
        private void handleFileInput(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "CSV Files(*.csv)|*.csv|All files (*.*)|*.*";

            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dlg.FileName;
                filepath.Text = filename;
                var reader = new StreamReader(File.OpenRead(@filename));
                var line = reader.ReadLine();
                tickers = line.Split(',');
            }

        }

        private void dataLoad(object sender, RoutedEventArgs e)
        {
            //preparing user inputed data
            DateTime begin = (DateTime)Date1.SelectedDate;
            DateTime end = (DateTime)Date2.SelectedDate;
            
            string start = begin.Year.ToString() + '-' + begin.Month.ToString("D2") + '-' + begin.Day.ToString("D2");
            string stop = end.Year.ToString() + '-' + end.Month.ToString("D2") + '-' + end.Day.ToString("D2");

            
            //creating the URL string with query parameters
            
            string yql_base_uri = "https://query.yahooapis.com/v1/public/yql";
            string yql_query = String.Format("?q=select%20*%20from%20yahoo.finance.historicaldata%20where%20symbol%20%3D%20%22{0}"+
                                             "%22%20and%20startDate%3D%22{1}%22%20and%20endDate%3D%22{2}%22"+
                                             "&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys", tickers[2], start, stop);
            string url = yql_base_uri + yql_query;
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(
              url);
           
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            //Load into XML tree to traverse,read, and load into DataTable
            XElement xmlDoc = XElement.Parse(@responseFromServer);
            XElement result = xmlDoc.Element("results");
            IEnumerable<XElement> quotes = result.Elements();
            //preparing the in memory table to load into TickerData table in SQL Server
            DataTable payload = new DataTable();
            payload.Columns.Add("Ticker", typeof(String));
            payload.Columns.Add("Date", typeof(DateTime));
            payload.Columns.Add("Time", typeof(String));
            payload.Columns.Add("Price", typeof(Decimal));

            foreach (XElement x in quotes)
            {
                object[] record = new object[] { x.Attribute("Symbol").Value, 
                                                Convert.ToDateTime(x.Element("Date").Value) ,
                                                "16:00:00",
                                                Decimal.Parse(x.Element("Close").Value) };
                payload.LoadDataRow(record,true);
               
            }
            
            var connection = ConfigurationManager.ConnectionStrings["StocksEntities"].ConnectionString;

            using (SqlConnection destination = new SqlConnection(connection))
            {
                destination.Open();
                using (SqlBulkCopy loader = new SqlBulkCopy(destination))
                {
                    loader.DestinationTableName = "dbo.TickerData";
                    try
                    {
                        // Write from the source to the destination.
                        loader.WriteToServer(payload);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }

      
            
            
             
        }
    }
}
