using System.Dynamic;
using System.Runtime.CompilerServices;

internal class Program
{
    // Initializing dictionary of distressing options to use throughout program
    // Using Keyword as key and percent change as value
    private static Dictionary<string, double> DistressingOptions = new()
    {
        {"UNFIN", -.05},
        {"SBI", .1},
        {"PBI", .1},
        {"FININTSP", .2},
        {"FININTAC", .3},
        {"ACIB", .2},
        {"PRIME/SV", 0},
        {"PRIME", 0},
        {"EASED", .03},
        {"RUB", .03},
        {"DENTS", .03},
        {"SPLITS", .03},
        {"WORMHOLES", .03},
        {"RASP", .03},
        {"SPATTER", .03},
        {"COWTAIL", .03},
        {"WBRUSH", .08}
    };
    private static void Main(string[] args)
    {
        while (true)
        {
            // Get Base Price from user input and parse for double
            Console.WriteLine("Input Base Price >");
            double.TryParse(Console.ReadLine(), out double basePrice);

            // Get Options from user input and convert to upper to avoid casing errors
            Console.WriteLine("Input Distressing Options (separated by commas) >");
            var optionsStr = Console.ReadLine().ToUpper();
            // Check for and remove any blank spaces to avoid matching errors
            if (optionsStr.Contains(" ")) optionsStr = optionsStr.Replace(" ", "");

            // If multiple options given, split into array
            // If not, make array with single input
            var optionsArr = optionsStr.Contains(",") ? optionsStr.Split(",") : new[] {optionsStr};

            // Declare output message (will contain error message or successfully calculated price)
            string outputMessage;

            // Confirm that inputs are valid before running them
            if (!OptionsValid(optionsArr)) outputMessage = "Please enter valid options";
            // Confirm that base price was valid input
            else if (basePrice < 0) outputMessage = "Please input valid Base Price";
            // Confirm that base price is valid (invalid input will parse to 0) and greater than $300
            else if (basePrice < 300) outputMessage = "Please input valid Base Price (must be over $300 & numeric values only)";
            // If input data is valid, run GetPrice() and format to dollar amount
            else outputMessage = "Final Price: $" + GetPrice(basePrice, optionsArr).ToString("0.00");

            // Output message
            Console.WriteLine(outputMessage);
            Thread.Sleep(1000);
            Console.WriteLine();
        }
    }
    private static bool OptionsValid(string[] options)
    {
        // Iterate over inputted (is that a word?) options
        foreach(var option in options)
        {
            // If master DistressingOptions dictionary does not contain one of the
            // user input options, return false
            if (!DistressingOptions.ContainsKey(option)) return false;
        }
        return true;
    }

    private static double GetPrice(double basePrice, string[] options)
    {
        // Initiate two add on prices. One for initial distressing charge, one for each additional
        double distressingCharge = 0;
        double multipleCharge = 0;

        // This will find the most expensive distressing charge of the inputs
        // Linq query to order the options by distressing charge and select the first one
        var highestDistressingOption = options.OrderByDescending(o => DistressingOptions[o]).First();
        distressingCharge = DistressingOptions[highestDistressingOption];

        foreach (var option in options)
        {
            // For each additional option, add 1%
            if (option != highestDistressingOption) multipleCharge += .01;
        }

        // Return the base price + the additional charges
        return basePrice + basePrice * (distressingCharge + multipleCharge);
    }
}
