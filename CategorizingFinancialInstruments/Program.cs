// See https://aka.ms/new-console-template for more information
using CategorizingFinancialInstruments;
using System.Text.Json;

string rangeFileName = "FinancialInstrumentCategories.json";
string instrumentFileName = "FinancialInstrument.json";

var ranges = new List<FinancialInstrumentCategory>();
var instruments = new List<FinancialInstrument>();

if (!File.Exists(rangeFileName))
    SaveRanges();

if (!File.Exists(instrumentFileName))
    SaveInstruments();

ReadRanges();
ReadInstruments();

if (args.Length > 0)
{
    string category = "";
    switch (args[0].ToLower())
    {
        case "addrange":
            AddRange(args);
            break;
        case "removerange":
            RemoveRange(args);
            break;
        case "addinstrument":
        case "add-instrument":
        case "add-i":
            AddInstrument(args);
            break;
        case "removeinstrument":
        case "remove-instrument":
        case "rm-instrument":
        case "rm-i":
            RemoveInstrument(args);
            break;
        case "show":
            Show(args); 
            break;
        default:
            Abandon("Parameter not recognized!");
            break;
    }
}

ShowOutput();
Success("End!");

void SaveRanges()
{
    string jsonString = JsonSerializer.Serialize(ranges);
    File.WriteAllText(rangeFileName, jsonString);
}

void ReadRanges()
{
    try
    {
        ranges = JsonSerializer.Deserialize<List<FinancialInstrumentCategory>>(File.ReadAllText(rangeFileName));
    }
    catch (Exception ex)
    {
        Abandon(ex.Message);
    }
}

void AddRange(string[] args)
{
    if (args.Length != 4)
        Abandon("Please use paramters 'addrange <rangename> <startvalue> <endvalue>' to add a range");

    var category = args[1];
    string message = "";

    double? startMarketValue = null;
    double? endMarketValue = null;

    var range = ranges.Where(x => x.Category == category).FirstOrDefault();

    if (double.TryParse(args[2], out double value))
        startMarketValue = value;

    if (double.TryParse(args[3], out double value2))
        endMarketValue = value2;

    if (startMarketValue is null)
        Abandon("Start Market Value has to be a valid number!");

    if (endMarketValue is null)
        Abandon("End Market Value has to be a valid number!");

    if (range != null)
    {
        range.StartMarketValue = startMarketValue.Value;
        range.EndMarketValue = endMarketValue.Value;
        message = $"Range {category} succesfuly updated!";
    }
    else
    {
        ranges.Add(new FinancialInstrumentCategory
        {
            Category = category,
            StartMarketValue = startMarketValue.Value,
            EndMarketValue = endMarketValue.Value
        });
        message = $"Range {category} succesfuly added!";
    }
    SaveRanges();
    Success(message);
}

void RemoveRange(string[] args)
{
    if (args.Length != 2)
        Abandon("Please use parameters 'removerange <rangename>' to remove it");

    var category = args[1];

    var remove = ranges.Where(x => x.Category == category).FirstOrDefault();

    if (remove is null)
        Abandon("Range not found!");

    ranges.Remove(remove);

    SaveRanges();

    Success($"Range {category} succesfuly removed!");
}

void ReadInstruments()
{
    try
    {
        instruments = JsonSerializer.Deserialize<List<FinancialInstrument>>(File.ReadAllText(instrumentFileName));
    }
    catch (Exception ex)
    {
        Abandon(ex.Message);
    }
}

void AddInstrument(string[] args)
{
    if (args.Length != 3)
        Abandon("Please use paramters 'addinstrument <type> <marketValue>' to add a instrument");

    var type = args[1];

    var instrument = instruments.Where(x => x.Type == type).FirstOrDefault();

    if (instrument != null)
        Abandon("Instrument Type already exists and cannot be updated!");

    double? marketValue = null;

    if (double.TryParse(args[2], out double value))
        marketValue = value;

    if (marketValue is null)
        Abandon("<marketValue> is invalid!");

    instruments.Add(new FinancialInstrument(type, marketValue.Value));

    SaveInstruments();

    Success($"Instrument {type} succesfuly added!");
}

void SaveInstruments()
{
    string jsonString = JsonSerializer.Serialize(instruments);
    File.WriteAllText(instrumentFileName, jsonString);
}

void Abandon(string? message = null)
{
    if (!string.IsNullOrEmpty(message))
        Console.WriteLine(message);

    Environment.Exit(1);
}

void Success(string? message = null)
{
    if (!string.IsNullOrEmpty(message))
        Console.WriteLine(message);

    Environment.Exit(0);
}

void RemoveInstrument(string[] args)
{
    if (args.Length != 2)
        Abandon("Please use parameters 'removeinstrument <type>' to remove it");

    var type = args[1];

    var instrument = instruments.Where(x => x.Type == type).FirstOrDefault();

    if (instrument is null)
        Abandon("Instrument not found!");

    instruments.Remove(instrument);

    SaveInstruments();

    Success($"Instrument {type} succesfuly removed!");
}

void Show(string[] args)
{
    if (args.Length != 2)
        Abandon("Please use parameters 'show instruments' or 'show ranges' to show its table");

    switch (args[1].ToLower())
    {
        case "instruments":
            ShowInstruments();
            break;
        case "ranges":
            ShowRanges();
            break;
        default:
            Abandon("Please use parameters 'show instruments' or 'show ranges' to show its table");
            break;
    }
}

void ShowRanges()
{
    Console.Clear();
    Console.WriteLine("Type                         Start Value           End Value");
    Console.WriteLine("--------------------  ------------------  ------------------");
    ranges.ForEach(item =>
    {
        Console.WriteLine($"{item.Category.PadRight(20)}  {item.StartMarketValue.ToString().PadLeft(18)}  {item.EndMarketValue.ToString().PadLeft(18)}");
    });
    Console.WriteLine("");
    Success($"Total instruments: {ranges.Count()}");
}

void ShowInstruments()
{
    Console.Clear();
    Console.WriteLine("Category                     MarketValue");
    Console.WriteLine("--------------------  ------------------");
    instruments.ForEach(item =>
    {
        Console.WriteLine($"{item.Type.PadRight(20)}  {item.MarketValue.ToString().PadLeft(18)}");
    });
    Console.WriteLine("");
    Success($"Total instruments: {instruments.Count()}");
}

void ShowOutput()
{
    var list = new List<string>();

    instruments.ForEach(item =>
    {
        var range = ranges.Where(x => item.MarketValue >= x.StartMarketValue && item.MarketValue <= x.EndMarketValue).FirstOrDefault();
        if (range is null)
            list.Add("Unknow");
        else
            list.Add(range.Category);
    });

    Console.Clear();
    Console.WriteLine($"instrumentsCategories = {{\"{ string.Join("\",\"", list.ToArray()) }\"}}");
}