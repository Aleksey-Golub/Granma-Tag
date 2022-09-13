using Assets.CodeBase.Emenies;
using Assets.CodeBase.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CodeBase.UI.EnemyIcons
{
    public class EnemiesIconsViewer : MonoBehaviour
    {
        [SerializeField] private bool _showEnemiesIcons;
        [SerializeField] private EnemyIcon _enemyIconPrefab;
        [Tooltip("Offset in")]
        [SerializeField] private float _iconOffset = 100f;

        private readonly Plane[] _planes = new Plane[6];
        private readonly List<EnemyIcon> _icons = new List<EnemyIcon>();
        private PlayerController _player;
        private List<EnemyOnLevel> _enemiesOnLevel;

        public void Initialize(List<EnemyOnLevel> enemiesOnLevel, PlayerController player)
        {
            _player = player;
            ClearIcons();

            _enemiesOnLevel = enemiesOnLevel;
            foreach (var enemy in _enemiesOnLevel)
            {
                var enemyIcon = Instantiate(_enemyIconPrefab, transform);
                enemyIcon.Init(enemy.EnemySprite);
                enemyIcon.Hide();

                _icons.Add(enemyIcon);
            }
        }

        public void Tick(float deltaTime)
        {
            if (_enemiesOnLevel == null)
                return;
            if (_player == null)
                return;
            
            var camera = Camera.main;
            GeometryUtility.CalculateFrustumPlanes(camera, _planes);
            for (int e = 0; e < _enemiesOnLevel.Count; e++)
            {
                EnemyOnLevel enemyOnLevel = _enemiesOnLevel[e];
                if (enemyOnLevel.Enemy.IsAlive == false)
                {
                    _icons[e].Hide();
                    continue;
                }

                Vector3 position = enemyOnLevel.Enemy.transform.position;
                Vector3 position1 = _player.transform.position;
                Vector3 toEnemy = position - position1;
                Ray ray = new Ray(_player.transform.position, toEnemy);

                float minDistance = float.MaxValue;
                int index = 0;
                // [0] = Left, [1] = Right, [2] = Down, [3] = Up
                for (int i = 0; i < 4; i++)
                {
                    if (_planes[i].Raycast(ray, out float distance))
                    {
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            index = i;
                        }
                    }
                }

                minDistance = Mathf.Clamp(minDistance, 0, toEnemy.magnitude);
                Vector3 worldPos = ray.GetPoint(minDistance);
                Vector3 pos = camera.WorldToScreenPoint(worldPos) + GetOffset(index);

                if (toEnemy.magnitude > minDistance)
                    _icons[e].Show();
                else
                    _icons[e].Hide();

                _icons[e].SetPosition(pos);
            }
        }

        public void HideIcons()
        {
            foreach (var icon in _icons)
                icon.Hide();
        }

        private Vector3 GetOffset(int index)
        {
            float offset = (float)Screen.width / 1440 * _iconOffset;
            // [0] = Left, [1] = Right, [2] = Down, [3] = Up
            return index switch
            {
                0 => new Vector3(offset, 0, 0),
                1 => new Vector3(-offset, 0, 0),
                2 => new Vector3(0, offset, 0),
                3 => new Vector3(0, -offset, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(index)),
            };
        }

        private void ClearIcons()
        {
            for (int i = 0; i < _icons.Count; i++)
                Destroy(_icons[i].gameObject);
            _icons.Clear();
        }
    }
}