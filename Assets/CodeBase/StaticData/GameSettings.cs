using Assets.CodeBase.Datas;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [SerializeField, Min(0)] private int _abilityCost = 30;
    [SerializeField] private List<LevelData> _levelsDatas;

    public IReadOnlyList<LevelData> LevelsDatas => _levelsDatas;
    public int AbilityCost => _abilityCost;
}

[Serializable]
public class LevelData
{
    public string SceneName;
    [Min(1)] public float Duration = 20f;
    [Min(0)] public int Reward = 10;
    public Stats EnemyStats;
}
