using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.Emenies.EnemyInput
{
    public class Route : MonoBehaviour
    {
        [field: SerializeField] public List<WayPoint> WayPoints { get; private set; }
    }
}
