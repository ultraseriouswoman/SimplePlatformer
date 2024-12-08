using UnityEngine;

public class FollowingLightRig : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Transform _character;
    [SerializeField] private float _smoothTime = 0f;

    private Vector3 _vel;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _character.position, ref _vel, _smoothTime);
        transform.forward  = Vector3.SmoothDamp(transform.forward, _character.forward, ref _vel, _smoothTime);

        _light.transform.LookAt(_character.position);
    }
}
