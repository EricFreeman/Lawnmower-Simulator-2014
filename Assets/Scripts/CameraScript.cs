using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Target;
    public float Distance = 5f;

	void Update ()
	{
	    transform.position = Target.position + (Vector3.up*Distance);
	}
}
