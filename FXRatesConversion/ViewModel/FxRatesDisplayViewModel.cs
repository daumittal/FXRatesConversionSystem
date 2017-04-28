using System;
using System.Collections.Generic;
using System.Linq;
using FXRateService;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace FXRatesConversion.ViewModel
{
    public class FxRatesDisplayViewModel
    {
        private readonly LiveFXRatesService ratesService;
        private readonly ConcurrentDictionary<string, RateViewModel> rateDictionary;
        private readonly object locker = new object();

        public FxRatesDisplayViewModel()
        {
            Records = new ObservableCollection<RecordViewModel>();
            rateDictionary = new ConcurrentDictionary<string, RateViewModel>();
            PopulateGridDefaults();

            ratesService = new LiveFXRatesService(GetAllCurrency());
            ratesService.FxRateUpdate += RatesService_FxRateUpdate;
            ratesService.Start();
        }

        public ObservableCollection<RecordViewModel> Records { get; set; }

        private void RatesService_FxRateUpdate(FXRateService.FxRateDetails rate)
        {
            lock(locker)
            {
                RateViewModel exisistingRate;
                if (rateDictionary.TryGetValue(rate.FromCurrency + rate.ToCurrency, out exisistingRate))
                {
                    exisistingRate.UpdateCount += 1;

                    if (exisistingRate.UpdateCount == 1)
                    {
                        exisistingRate.Rate = rate.Rate;
                        exisistingRate.UpdateTime = rate.UpdateTime;
                        return;
                    }
                    else if(exisistingRate.Rate== rate.Rate && exisistingRate.UpdateCount % 3==0)
                    {
                        exisistingRate.Rate = rate.Rate+0.01;
                        exisistingRate.UpdateTime = rate.UpdateTime;
                    }
                    else
                    {
                        exisistingRate.Rate = rate.Rate;
                        exisistingRate.UpdateTime = rate.UpdateTime;
                    }

                    //Apply calculation to change currency rates for all other currencies conversion using ToCurrency

                    //Update all currencies in row
                    var dependentCurrenciesInRow = rateDictionary.Values.Where(v => v.FromCurrency == rate.ToCurrency).ToList();

                    if (dependentCurrenciesInRow != null && dependentCurrenciesInRow.Count > 0)
                    {
                        foreach (var cur in dependentCurrenciesInRow)
                        {
                            RateViewModel exisistingRate2;
                            if (rateDictionary.TryGetValue("USD" + cur.ToCurrency, out exisistingRate2))
                            {
                                try
                                {
                                    cur.Rate = exisistingRate2.Rate / exisistingRate.Rate;
                                    cur.UpdateTime = exisistingRate.UpdateTime;
                                }
                                catch (DivideByZeroException)
                                {
                                }
                            }
                        }
                    }

                    //update all currencies in columns
                    var dependentCurrenciesInColumn = rateDictionary.Values.Where(v => v.ToCurrency == rate.ToCurrency && v.ToCurrency!="USD").ToList();

                    if (dependentCurrenciesInColumn != null && dependentCurrenciesInColumn.Count > 0)
                    {
                        foreach (var cur in dependentCurrenciesInColumn)
                        {
                            RateViewModel exisistingRate2;
                            if (rateDictionary.TryGetValue("USD" + cur.FromCurrency, out exisistingRate2))
                            {
                                try
                                {
                                    cur.Rate = exisistingRate.Rate / exisistingRate2.Rate;
                                    cur.UpdateTime = exisistingRate.UpdateTime;
                                }
                                catch (DivideByZeroException)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<string> GetAllCurrency()
        {
            List<string> lstCurrency = new List<string>();
            lstCurrency.Add("USD");
            lstCurrency.Add("GBP");
            lstCurrency.Add("EUR");
            lstCurrency.Add("AUD");
            lstCurrency.Add("CAD");
            lstCurrency.Add("ZAR");
            lstCurrency.Add("JPY");
            lstCurrency.Add("INR");
            lstCurrency.Add("CNY");
            lstCurrency.Add("NZD");
            lstCurrency.Add("CHF");
            lstCurrency.Add("HKD");

            return lstCurrency;
        }

        private void PopulateGridDefaults()
        {
            foreach (var fromCurrency in GetAllCurrency())
            {
                var recordViewModel = new RecordViewModel();

                var rateViewModelDummy = new RateViewModel();
                rateViewModelDummy.FromCurrency = fromCurrency;
                recordViewModel.RateViewModels.Add(rateViewModelDummy);

                foreach (var toCurrency in GetAllCurrency())
                {
                    var rateViewModel = new RateViewModel();
                    rateViewModel.FromCurrency = fromCurrency;
                    rateViewModel.ToCurrency = toCurrency;

                    if (fromCurrency == toCurrency)
                        rateViewModel.Rate = 1;
                    else
                        rateViewModel.Rate = null;

                    rateViewModel.UpdateTime = DateTime.Now;
                    recordViewModel.RateViewModels.Add(rateViewModel);
                    rateDictionary.TryAdd(rateViewModel.FromCurrency + rateViewModel.ToCurrency, rateViewModel);
                }

                Records.Add(recordViewModel);
            }
        }
    }
}

