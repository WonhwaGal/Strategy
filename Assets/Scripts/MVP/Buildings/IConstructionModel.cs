using Code.Units;
using System;

namespace Code.Construction
{
    public interface IConstructionModel : IModel
    {
        public int ID { get; }
        public int CurrentStage { get; }
        public int TotalStages { get; }
        public bool[] AutoUpgrades { get; }
        public int[] PriceList { get; }
    }
}