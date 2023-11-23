using System;
using UnityEngine;
using Code.Construction;
using Code.Input;
using static WaveSO;

namespace Code.Combat
{
    public class LevelService : IService, IDisposable
    {
        private readonly IInputService _input;
        private readonly ConstructionService _construction;
        private readonly WaveSO _wavesData;
        private int _level = 1;
        private int _waveTurn = 1;

        public LevelService(WaveSO wavesData)
        {
            _wavesData = wavesData;
            _construction = ServiceLocator.Container.RequestFor<ConstructionService>();
            WaveLocator.OnWaveEnd += CheckNextWave;
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            SubscribeToSpaceKey(isNight: false);
        }

        public int Level
        {
            get => _level;
            private set
            {
                if (value > _level)
                    _waveTurn = 1;
                _level = value;
            }
        }

        public event Action<LevelWaveData, int> OnCreatingWaves;
        public event Action<GameMode> OnChangingGameMode;

        private void SubscribeToSpaceKey(bool isNight)
        {
            if (!isNight)
            {
                _input.OnPressSpace += OnSpacePressed;
                _input.OnPressCtrl += OnLeftCtrlPressed;
            }
            else
            {
                _input.OnPressSpace -= OnSpacePressed;
                _input.OnPressCtrl -= OnLeftCtrlPressed;
            }
        }

        private void OnSpacePressed()
        {
            if (!_construction.ReadyToBuild())
                InvokeWave();
        }

        private void InvokeWave()
        {
            SubscribeToSpaceKey(isNight: true);
            OnCreatingWaves?.Invoke(_wavesData.FindLevelWaves(Level), _waveTurn);
            OnChangingGameMode?.Invoke(GameMode.IsNight);
            _waveTurn++;
        }

        private void OnLeftCtrlPressed(bool startControl)
        {
            var mode = startControl ? GameMode.IsUnitControl : GameMode.IsDay;
            OnChangingGameMode?.Invoke(mode);
        }



        private void CheckNextWave(bool isVictory)
        {
            if (!isVictory)
            {
                SwitchToDay(isVictory);
            }
            else
            {
                if (_waveTurn > _wavesData.FindLevelWaves(Level).WavesCount())
                    SwitchToDay(isVictory);
                else
                    InvokeWave();
            }
        }

        public void SwitchToDay(bool isVictory)
        {
            SubscribeToSpaceKey(isNight: false);
            if (isVictory)
            {
                Debug.Log("Level is won");
                Level++;
                _construction.StartLevel(Level);
                OnChangingGameMode?.Invoke(GameMode.IsDay);
            }
            else
            {
                Debug.Log("Castle is destroyed, game is lost");
                OnChangingGameMode?.Invoke(GameMode.IsGameLost);
            }
        }

        public void Dispose()
        {
            WaveLocator.OnWaveEnd -= CheckNextWave;
            _input.OnPressSpace -= OnSpacePressed;
            _input.OnPressCtrl -= OnLeftCtrlPressed;
        }
    }
}