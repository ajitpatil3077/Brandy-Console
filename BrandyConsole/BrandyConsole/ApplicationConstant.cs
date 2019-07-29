namespace BrandyConsole.Generators
{
    public static class ApplicationConstant
    {
        //Folder and File paths
        public const string INPUT_FOLDERPATH = "InputFolderPath";

        public const string REFERENCE_DATA_FILEPATH = "ReferenceDataFilePath";
        public const string INPUT_FILEPATH = "InputFilePath";
        public const string OUTPUT_FILEPATH = "OutputFilePath";

        //Input file constants
        public const string DAY = "Day";
        public const string NAME = "Name";
        public const string TOTAL_HEAT_INPUT = "TotalHeatInput";
        public const string ACTUAL_NET_GENERATION = "ActualNetGeneration";
        public const string EMISSIONS_RATING = "EmissionsRating";
        public const string COAL_GENERATOR = "CoalGenerator";
        public const string GAS_GENERATOR = "GasGenerator";
        public const string WIND_GENERATOR = "WindGenerator";
        public const string WIND_OFFSHORE_GENERATOR = @"Wind[Offshore]";
        public const string WIND_ONSHORE_GENERATOR = @"Wind[Onshore]";
        public const string LOCATION = "Location";
        
        public const string LOCATION_ONSHORE_WIND_GENERATOR = "Onshore";
        public const string LOCATION_OFFSHORE_WIND_GENERATOR = "Offshore";

        public const string DATE = "Date";
        public const string ENERGY = "Energy";
        public const string PRICE = "Price";

        //Output File constants
        
        public const string GENERATION_OUTPUT = "GenerationOutput";
        public const string TOTALS = "Totals";
        public const string MAX_EMISSION_GENERATORS = "MaxEmissionGenerators";
        public const string ACTUAL_HEAT_RATES = "ActualHeatRates";

        public const string GENERATOR = "Generator";
        public const string TOTAL = "Total";
        public const string HEAT_RATE = "HeatRate";
        public const string EMISSION = "Emission";
        public const string DATE_FORMAT = "O";
        
        //ReferenceData file constants
        public const string VALUE_FACTOR = "ValueFactor";
        public const string HIGH = "High";
        public const string MEDIUM = "Medium";
        public const string LOW = "Low";
        public const string EMISSIONS_FACTOR = "EmissionsFactor";

        //Error Messages
        public const string INPUT_FILE_NOT_FOUND = "Input file path not provided.";
        public const string REFERENCE_DATA_FILE_NOT_FOUND = "Reference Data file path not provided.";

        //
    }
}
