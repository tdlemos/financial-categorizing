using CategorizingFinancialInstruments.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategorizingFinancialInstruments;

public class FinancialInstrument : IFinancialInstrument
{
    public double MarketValue { get; }

    public string Type { get; }

    public FinancialInstrument(string type, double marketValue)
    {
        Type = type;
        MarketValue = marketValue;
    }
}
