using UnityEngine;

public class CameraRigScript : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _character;
    [SerializeField] private float _smoothTime = 0.275f;

    private Vector3 _vel;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _character.position, ref _vel, _smoothTime);
        transform.forward  = Vector3.SmoothDamp(transform.forward, _character.forward, ref _vel, _smoothTime);

        _camera.transform.LookAt(_character.position);
    }
}
