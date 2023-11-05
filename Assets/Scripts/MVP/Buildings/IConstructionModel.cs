using System;
using UnityEngine;

namespace Code.Construction
{
    public interface IConstructionModel : IDisposable
    {
        public int ID { get; }
        public int CurrentStage { get; }
        public int TotalStages { get; }
        public bool[] AutoUpgrades { get; }
        public int[] PriceList { get; }
    }
}