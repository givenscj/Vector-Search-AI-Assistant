using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLoader
{
    public class DataLoader
    {
        public string ApiUrl { get; set; }

        public DataLoader(
            ILogger<DataLoader> logger)
        {

        }

        async public Task LoadData()
        {
            //load the product data from file

            //load the customer data.

            //load the order data.
        }
    }
}
