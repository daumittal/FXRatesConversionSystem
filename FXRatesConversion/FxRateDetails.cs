using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXRatesConversion
{
    public class FxRateDetails
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
