using UnityEngine;
using System.Collections;

public class SlideOnIntro : MonoBehaviour {

    public Vector3 target;
    public float speed;
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

}
