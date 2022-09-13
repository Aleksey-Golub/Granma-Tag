using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.UI.EnemyIcons
{
    public class EnemyIcon : MonoBehaviour
    {
        [SerializeField] private Image _image;

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void Init(Sprite bullySprite) => _image.sprite = bullySprite;

        public void SetPosition(Vector3 pos) => transform.position = pos;
    }
}
