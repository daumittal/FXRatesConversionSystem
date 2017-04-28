using System;

namespace FXRateService
{
    public class FxRateDetails
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Rate { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
