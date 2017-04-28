using System;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FXRateService
{
    public class GetLiveRates
    {
        public async Task<FxRateDetails> Parse(string fromCurrency, string toCurrency, string url)
        {
            try
            {
                WebClient web = new WebClient();
                string response = await web.DownloadStringTaskAsync(url);
                //string[] values = Regex.Split(response,",");
                //double rate = System.Convert.ToDouble(values[1]);

                var matches = Regex.Matches(response, "<span class=\"?bld\"?>([^<]+)</span>");

                if (matches.Count>0)
                {
                    var matches2 = Regex.Split(matches[0].Groups[1].Value, @"[^0-9\.]+");

                    if (matches2!=null && matches2.Length>1)
                    {
                        var rate = Convert.ToDouble(matches2[0]);

                        var fxRateDetails = new FxRateDetails();
                        fxRateDetails.FromCurrency = fromCurrency;
                        fxRateDetails.ToCurrency = toCurrency;
                        fxRateDetails.Rate = rate;
                        return fxRateDetails;
                    }
                }
            }
            catch (Exception)
            {
                // ignored, if exception
            }
            return null;
        }
    }
}
