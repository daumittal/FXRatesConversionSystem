using System.Timers;
using System.Collections.Generic;

namespace FXRateService
{
    public class LiveFXRatesService
    {
        private List<string> currencies;
        private const string baseCurrency = "USD";
        public delegate void FxRateUpdateHadler(FxRateDetails rate);
        public event FxRateUpdateHadler FxRateUpdate;

        public LiveFXRatesService(List<string> currenciesList)
        {
            currencies = currenciesList;
        }

        public void Start()
        {
            Timer timer = new Timer(5000);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Enabled = true;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var currency in currencies)
            {
                if (currency == "USD")
                    continue;

                // string url = string.Format("https://finance.yahoo.com/d/quotes.csv?e=.csv&f=sl1d1t1&s={0}{1}=X", baseCurrency, currency);
                string url = string.Format("https://www.google.com/finance/converter?a={0}&from={1}&to={2}&meta={3}", 1, baseCurrency, currency, System.Guid.NewGuid().ToString());

                StartTask(baseCurrency, currency,url);
            }
        }

        private async void StartTask(string fromCurrency, string toCurrency,string url)
        {
            var parser = new GetLiveRates();
            var fxRateDetails = await parser.Parse(fromCurrency, toCurrency,url);

            if (fxRateDetails != null)
            {
                FxRateUpdate(fxRateDetails);
            }
        }
    }
}


