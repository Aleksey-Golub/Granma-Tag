using UnityEngine;

namespace Assets.CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private Game _gamePrefab;

        private void Awake()
        {
            var game = FindObjectOfType<Game>();

            if (game != null) 
                return;

            var gameGO = Instantiate(_gamePrefab);
        }
    }
}
