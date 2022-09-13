using Assets.CodeBase.Player;
using Assets.CodeBase.Services.Input;
using UnityEngine;

namespace Assets.CodeBase.Emenies.EnemyInput
{
    public class GranmaInput : MonoBehaviour, IInputService
    {
        private PlayerController _player;

        public Vector2 RawAxis => GetMoveDirection();

        public void Init(PlayerController player) => _player = player;

        private Vector2 GetMoveDirection()
        {
            Vector3 toPlayer = _player.transform.position - transform.position;
            float x = Mathf.Clamp(toPlayer.x, -1, 1);
            float z = Mathf.Clamp(toPlayer.z, -1, 1);

            return new Vector2(x, z);
        }
    }
}
