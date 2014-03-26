using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float PlayerMoveSpeed = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.MovePosition(rigidbody.position + new Vector3(
            Input.GetAxisRaw("Horizontal") * PlayerMoveSpeed, 
            0, 
            Input.GetAxisRaw("Vertical") * PlayerMoveSpeed) * Time.deltaTime);	
	}
}
