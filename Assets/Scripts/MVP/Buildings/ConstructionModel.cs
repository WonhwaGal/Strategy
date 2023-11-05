using System;
using UnityEngine;
using Code.ScriptableObjects;

namespace Code.Construction
{
    public class ConstructionModel : IConstructionModel
    {
        private int _defense;

        public int Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                if (_defense <= 0)
                    OnDestroyed?.Invoke();
            }
        }
        public int ID { get; set; }
        public int ActivatedBy { get; set; }
        public bool AutoVisible { get; set; }
        public int CurrentStage { get; set; } = -1;
        public int TotalStages { get; set; }
        public bool[] AutoUpgrades { get; set; }
        public int[] PriceList { get; set; }

        public event Action OnDestroyed;

        public ConstructionModel(SingleBuildingData data)
        {
            _defense = data.CommonInfo.Defense;
            AutoUpgrades = data.CommonInfo.AutoUpgrades;
            TotalStages = data.CommonInfo.TotalStages;
            PriceList = data.CommonInfo.PriceList;
            ID = data.UniqueInfo.ID;
            ActivatedBy = data.UniqueInfo.ActivatedBy;
            AutoVisible = data.UniqueInfo.AutoVisible;
        }

        public object Clone() => this.MemberwiseClone();

        public void Dispose() { }
    }
}