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
using Microsoft.Office.Interop.Excel;
using System.Collections;
using correlation_matrix.ServiceReference1;
using StatisticsAPI;


namespace correlation_matrix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Generates stock correlation matrix from user input params: tickers and daterange
        private void GenerateExcelFile(object sender, RoutedEventArgs e)
        {
            //harvesting user input data for further processing
            string ticker = Tickers.Text;
            char[] delimit = { ',' };

            string[] tickerArray = ticker.Split(delimit);
            DateTime start = (DateTime)Date1.SelectedDate;
            DateTime end = (DateTime)Date2.SelectedDate;



            //creating the correlation matrix and storing it in var data
            int rows = tickerArray.Length + 1;
            int columns = rows;
            var data = new object[rows, columns];
            //preparing the database connection proxy
            DataBaseClient client = new DataBaseClient();
            //matrix is filled in row by row
            for (var row = 1; row <= rows; row++)
            {
                //first row is all ticker headers
                if (row == 1)
                {
                    for (var column = 1; column <= columns; column++)
                    {
                        if (column > 1)
                        {
                            data[row - 1, column - 1] = tickerArray[column - 2];
                        }
                    }
                }
                else
                //all other rows contain correlation coefficents
                {
                    decimal[] stock1 = client.getPriceArray(tickerArray[row - 2], start, end); ;
                    decimal[] stock2;
                    decimal coefficient;
                    for (var column = 1; column <= columns; column++)
                    {
                        if (column == 1)
                        {
                            data[row - 1, column - 1] = tickerArray[row - 2];
                        }
                        //gets price data through database proxy
                        //Then uses statsAPI to get coefficent
                        else if (column >= row)
                        {
                            if (column == row)
                            {
                                data[row - 1, column - 1] = 1;
                            }

                            else
                            {
                                stock2 = client.getPriceArray(tickerArray[column - 2], start, end);
                                coefficient = Statistics.correlation(stock1, stock2);

                                data[row - 1, column - 1] = coefficient;
                            }
                        }
                    }
                    
                }
                
            }
            client.Close();

            // Handles creation of excel workbook and loads the data all at once
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];
            var startCell = ws.Cells[1, 1];
            var endCell = ws.Cells[rows, columns];
            var writeRange = ws.Range[startCell, endCell];

            writeRange.Value2 = data;
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;
        }

    }
}