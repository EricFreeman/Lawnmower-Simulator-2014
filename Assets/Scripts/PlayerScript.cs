using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float PlayerMoveSpeed = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Input.GetAxisRaw("Horizontal") * PlayerMoveSpeed, 
                            0, 
                            Input.GetAxisRaw("Vertical") * PlayerMoveSpeed);	
	}
}
