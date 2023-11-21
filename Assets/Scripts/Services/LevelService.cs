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
        private bool _isNight;
        private int _waveTurn = 1;

        public LevelService(WaveSO wavesData)
        {
            _wavesData = wavesData;
            _construction = ServiceLocator.Container.RequestFor<ConstructionService>();
            WaveLocator.OnWaveEnd += CheckNextWave;
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            _input.OnPressSpace += OnSpacePressed;
            _input.OnPressCtrl += OnLeftCtrlPressed;
        }

        public int Level { get; private set; } = 1;

        public event Action<LevelWaveData, int> OnCreatingWaves;
        public event Action<GameMode> OnChangingGameMode;

        private void OnSpacePressed()
        {
            if (_isNight)
                return;

            _isNight = !_construction.ReadyToBuild();
            if (_isNight)
                InvokeWave();
        }

        private void OnLeftCtrlPressed(bool startControl)
        {
            var mode = startControl ? GameMode.IsUnitControl : GameMode.IsDay;
            OnChangingGameMode?.Invoke(mode);
        }

        private void InvokeWave()
        {
            OnCreatingWaves?.Invoke(_wavesData.FindLevelWaves(Level), _waveTurn);
            OnChangingGameMode?.Invoke(GameMode.IsNight);
            _waveTurn++;
        }

        private void CheckNextWave(bool isVictory)
        {
            if (isVictory == false)
                SwitchToDay(isVictory);
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
            _isNight = false;
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
            _input.OnPressSpace -= OnSpacePressed;
            _input.OnPressCtrl -= OnLeftCtrlPressed;
        }
    }
}