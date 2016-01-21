using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DatabaseAPI
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class DataBase : IDataBase
    {

        public decimal[] getPriceArray(String ticker, DateTime start, DateTime end)
        {
            decimal[] priceArray;
            
            using (StocksEntities context = new StocksEntities())
            {
                //sql script to extract price
                var query = from record in context.TickerDatas
                            where
                                record.Ticker == ticker &&
                                record.Date >= start &&
                                record.Date <= end
                            orderby  record.Date
                            select record.Price;

                priceArray = query.ToArray();
            
            }
             
            Console.WriteLine("Received Request for {0} from {1} to {2}", ticker, start, end);
            return priceArray;
       
        }

        
    }
}
