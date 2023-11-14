using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(WaveSO), menuName = "Waves/WaveSO")]
public partial class WaveSO : ScriptableObject
{
    [SerializeField] protected List<LevelWaveData> _waves;

    public LevelWaveData FindLevelWaves(int lvl)
    {
        var levelData = _waves.Find(x => x.Level == lvl);
        if (levelData != null)
            return levelData;
        else
            Debug.LogError($"{name} : level {lvl} is not found");

        return null;
    }

    [System.Serializable]
    public class LevelWaveData
    {
        public string Name;
        public int Level;
        public List<EnemySpawnData> SpawnSpots; 
    }
}