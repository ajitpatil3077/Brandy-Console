using BrandyConsole.DTO;
using System.Collections.Generic;

namespace BrandyConsole.Generators
{
    /// <summary>
    /// This class has all the details of gas generator.
    /// </summary>
    public class GasGenerator : GeneratorBase
    {
        #region constructor
        public GasGenerator(string filePath, string refDataFilePath) : base(filePath, refDataFilePath)
        {

        } 
        #endregion

        #region override methods
        /// <summary>
        /// This method sets the Name of generator to retreiving data for gas generator from input xml file.
        /// </summary>
        /// <param name="generatorName"></param>
        public override void SetGeneratorName(ref string generatorName)
        {
            generatorName = ApplicationConstant.GAS_GENERATOR;
        }

        /// <summary>
        /// Computes the required values based upon rules applied for coal generator.
        /// </summary>
        /// <returns></returns>
        public override List<GeneratorDTO> Calculate()
        {
            foreach (GeneratorDTO generatorDTO in generatorDTOList)
            {
                //ActualHeatRate = TotalHeatInput/ActualNetGeneration
                generatorDTO.ActualHeatRate = generatorDTO.TotalHeatInput / generatorDTO.ActualNetGeneration;

                foreach (DayDTO dayDTO in generatorDTO.Generation)
                {
                    //DailyGenerationValue = Energy * Price * ValueFactor(Medium) of all generations
                    generatorDTO.DailyGenerationValue = dayDTO.Energy * dayDTO.Price * referenceDataDTO.ValueFactorMedium + generatorDTO.DailyGenerationValue;

                    //DailyEmissionsValue = Energy * EmissionsRating * EmissionFactor(Medium) of all generations
                    dayDTO.DailyEmissionsValue = dayDTO.Energy * generatorDTO.EmissionsRating * referenceDataDTO.EmissionFactorMedium + dayDTO.DailyEmissionsValue;
                }
            }

            return generatorDTOList;
        } 
        #endregion
    }
}
