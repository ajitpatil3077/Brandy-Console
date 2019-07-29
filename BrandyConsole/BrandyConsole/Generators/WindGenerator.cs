using BrandyConsole.DTO;
using System.Collections.Generic;

namespace BrandyConsole.Generators
{
    /// <summary>
    /// This class has all the details of wind generator.
    /// </summary>
    public class WindGenerator : GeneratorBase
    {
        #region constructor
        public WindGenerator(string filePath, string refDataFilePath) : base(filePath, refDataFilePath)
        {
        } 
        #endregion

        #region override methods
        /// <summary>
        /// This method sets the Name of generator to retreiving data for wind generator from input xml file.
        /// </summary>
        /// <param name="generatorName"></param>
        public override void SetGeneratorName(ref string generatorName)
        {
            generatorName = ApplicationConstant.WIND_GENERATOR;
        }

        /// <summary>
        /// Computes the required values based upon rules applied for wind generator.
        /// </summary>
        /// <returns></returns>
        public override List<GeneratorDTO> Calculate()
        {
            foreach (GeneratorDTO generatorDTO in generatorDTOList)
            {
                foreach (DayDTO dayDTO in generatorDTO.Generation)
                {
                    // In case of offshore wind generator
                    if (generatorDTO.Location == ApplicationConstant.LOCATION_OFFSHORE_WIND_GENERATOR)
                    {
                        // DailyGenerationValue = Energy * Price * ValueFactor(Low) of all generations.
                        generatorDTO.DailyGenerationValue = dayDTO.Energy * dayDTO.Price * referenceDataDTO.ValueFactorLow + generatorDTO.DailyGenerationValue;
                    }
                    // in case of on shore wind generator
                    else if (generatorDTO.Location == ApplicationConstant.LOCATION_ONSHORE_WIND_GENERATOR)
                    {
                        // DailyGenerationValue = Energy * Price * ValueFactor(High) of all generations.
                        generatorDTO.DailyGenerationValue = dayDTO.Energy * dayDTO.Price * referenceDataDTO.ValueFactorHigh + generatorDTO.DailyGenerationValue;
                    }

                    //DailyEmissionsValue = Energy * EmissionsRating of all generations.
                    dayDTO.DailyEmissionsValue = dayDTO.Energy * generatorDTO.EmissionsRating + dayDTO.DailyEmissionsValue;
                }

                //ActualHeatRate = TotalHeatInput / ActualNetGeneration
                generatorDTO.ActualHeatRate = generatorDTO.TotalHeatInput / generatorDTO.ActualNetGeneration;
            }

            return generatorDTOList;
        } 
        #endregion

    }
}
