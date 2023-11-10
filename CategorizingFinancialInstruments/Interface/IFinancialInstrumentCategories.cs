namespace CategorizingFinancialInstruments.Interface;

public interface IFinancialInstrumentCategories
{
    double StartMarketValue { get; set; }
    double EndMarketValue { get; set; }
    string Category { get; set; }
}