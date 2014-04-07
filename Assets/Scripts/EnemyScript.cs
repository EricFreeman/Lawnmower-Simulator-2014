using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public float FieldOfView;

    private bool isAlerted;

    public EnemyState State = EnemyState.Patrolling;
    private Vector3 LastKnownLocation;

    void Start()
    {

    }

    void Update()
    {
        Vector3 rayDirection = Player.transform.position - transform.position;
 
       // Detect if player is within the field of view
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    renderer.material.color = Color.yellow;
                    Debug.Log("Can see player");
                    isAlerted = true;
                }
            }
        }
    }
}

public enum EnemyState
{
    Idle,
    Patrolling,
    Alert,
    Detect,
    Dead
}
