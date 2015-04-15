using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    // variables
    public float xMargin = 1f;
    public float yMargin = 1f;
    public float xSmooth = 1.1f;
    public float ySmooth = 1f;
    private Vector2 Velocity;
    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public Transform player;

	void Start () {
        // Setting up the reference to the player object
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

    void Update() {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        // get current x and y
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // if player moves beyond x
        if (CheckXMargin())
            targetX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref Velocity.x, xSmooth);

        // if player moves beyond y
        if (CheckYMargin())
            targetY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref Velocity.y, ySmooth);

        // if x and y are larger than set coordinates
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set camera position to target position with same z component
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    bool CheckXMargin() {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }
}
