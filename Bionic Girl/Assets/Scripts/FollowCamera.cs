using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public GameObject cameraTarget;
    public GameObject player;

    public float SmoothTime = 0.1f;
    public Vector2 CameraHeight = new Vector2(2.5f, 0.0f);
    public bool CameraFollowX = true;
    public bool CameraFollowY = true;
    public bool CameraFollowHeight = false;
    public Vector2 Velocity;

    private Transform _cameraTransform;
    private Camera _camera;

	void Start ()
	{
	    _camera = GetComponent<Camera>();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
	    _cameraTransform = transform;      
	}

	void Update () {
	    if(CameraFollowX);
            _cameraTransform.position = new Vector3(Mathf.SmoothDamp(_cameraTransform.position.x, cameraTarget.transform.position.x, ref Velocity.x, SmoothTime), _cameraTransform.position.y, -0.0f);
        if(CameraFollowY)
            _cameraTransform.position = new Vector3(_cameraTransform.position.x, Mathf.SmoothDamp(_cameraTransform.position.y, cameraTarget.transform.position.y, ref Velocity.y, SmoothTime), -0.0f);
	    if (CameraFollowHeight)
	        _camera.transform.position = CameraHeight;
	}
}
