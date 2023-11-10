using CategorizingFinancialInstruments.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategorizingFinancialInstruments;

public class FinancialInstrumentCategory : IFinancialInstrumentCategories
{
    public double StartMarketValue { get; set; }
    public double EndMarketValue { get; set; }
    public string Category { get; set; }
}
