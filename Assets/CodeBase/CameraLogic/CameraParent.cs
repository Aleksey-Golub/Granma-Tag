using UnityEngine;

namespace Assets.CodeBase.CameraLogic
{
    public class CameraParent : MonoBehaviour
    {
        [SerializeField] private bool _useSoftCamera;
        [SerializeField] private float _softCameraSpeed = 10f;

        private Transform _target;

        public void Construct(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target)
                transform.position =
                    _useSoftCamera 
                    ? UseLerp()
                    : transform.position = _target.position;
        }

        private Vector3 UseLerp() => Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _softCameraSpeed);
    }
}