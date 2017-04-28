using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FXRatesConversion.ViewModel
{
    public class RecordViewModel 
    {
        public ObservableCollection<RateViewModel> RateViewModels { get; set; }

        public RecordViewModel()
        {
            RateViewModels = new ObservableCollection<RateViewModel>();
        }
    }
}