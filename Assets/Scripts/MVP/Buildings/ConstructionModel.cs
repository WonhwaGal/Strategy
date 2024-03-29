﻿using System;
using UnityEngine;
using Code.ScriptableObjects;

namespace Code.Construction
{
    public class ConstructionModel : IConstructionModel
    {
        private int _defense;
        private bool _isDestroyed;

        public ConstructionModel(SingleBuildingData data)
        {
            _defense = MaxHP = data.CommonInfo.Defense;
            PrefabType = data.PrefabType;
            Transform = data.Transform;
            IsForCombat = data.CommonInfo.IsForCombat;
            DamageRadius = data.CommonInfo.DamageRadius;
            LongRadius = data.CommonInfo.AttackRadius;
            AttackInterval = data.CommonInfo.AttackInterval;
            AutoUpgrades = data.CommonInfo.AutoUpgrades;
            TotalStages = data.CommonInfo.TotalStages;
            PriceList = data.CommonInfo.PriceList;
            ID = data.UniqueInfo.ID;
            ActivatedBy = data.UniqueInfo.ActivatedBy;
            AutoVisible = data.UniqueInfo.AutoVisible;
        }

        public PrefabType PrefabType { get; private set; }
        public Transform Transform { get; private set; }
        public bool IsDestroyed
        {
            get => _isDestroyed;
            set
            {
                if(_isDestroyed && !value)
                    Defense = MaxHP;

                _isDestroyed = value;
            }
        }
        public int Defense
        {
            get => _defense;
            set
            {
                _defense = value;
                if (_defense <= 0)
                    OnKilled?.Invoke();
                if(_defense > MaxHP)
                    _defense = MaxHP;
            }
        }
        public int MaxHP { get; private set; }
        public bool IsForCombat { get; private set; }
        public float DamageRadius { get; private set; }
        public float LongRadius { get; private set; }
        public float AttackInterval { get; private set; }
        public int ID { get; set; }
        public int ActivatedBy { get; set; }
        public bool AutoVisible { get; set; }
        public int CurrentStage { get; set; } = -1;
        public int TotalStages { get; set; }
        public bool[] AutoUpgrades { get; set; }
        public int[] PriceList { get; set; }

        public event Action OnKilled;

        public void Dispose() { }
    }
}