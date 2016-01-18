using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DatabaseAPI
{
    [ServiceContract]
    public interface IDataBase
    {
        //returns time ordered price array of stock
        [OperationContract]
        decimal[] getPriceArray(String ticker, DateTime start, DateTime end);
        
    }
}
