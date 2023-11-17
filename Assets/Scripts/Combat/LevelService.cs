using System;
using Code.Construction;
using static WaveSO;

namespace Code.Combat
{
    public class LevelService : IService
    {
        private readonly ConstructionService _construction;
        private readonly WaveSO _wavesData;
        private bool _isNight;

        public LevelService(WaveSO wavesData)
        {
            _wavesData = wavesData;
            _construction = ServiceLocator.Container.RequestFor<ConstructionService>();
            WaveLocator.SubscribeToWaveOver(SwitchToDay);
        }

        public int Level { get; private set; } = 1;

        public event Action<LevelWaveData> OnCreatingWaves;
        public event Action<GameMode> OnChangingGameMode;

        public void OnSpacePressed()
        {
            if (_isNight)
                return;

            _isNight = !_construction.ReadyToBuild();
            if (_isNight)
            {
                OnCreatingWaves?.Invoke(_wavesData.FindLevelWaves(Level));
                OnChangingGameMode?.Invoke(GameMode.IsNight);
            }
        }

        public void SwitchToDay(bool isVictory)
        {
            if(isVictory)
            {
                Level++;
                _construction.StartLevel(Level);
                OnChangingGameMode?.Invoke(GameMode.IsDay);
            }
        }
    }
}