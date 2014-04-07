using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public float FieldOfView = 60f;
    public float ViewDistance = 5.5f;
    public float MoveSpeed = 2f;

    private bool canSeePlayer;

    public EnemyState State = EnemyState.Patrolling;
    private Vector3 LastKnownLocation;

    void Start()
    {

    }

    void FixedUpdate()
    {
        canSeePlayer = false;
        Vector3 rayDirection = Player.transform.position - transform.position;

       // Detect if player is within the field of view
        if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit) && Vector3.Distance(transform.position, Player.transform.position) < ViewDistance)
                if (hit.transform.tag == "Player")
                    canSeePlayer = true;
        }

        switch (State)
        {
            case EnemyState.Idle:
                if(canSeePlayer) State = EnemyState.Alert;
                break;
            case EnemyState.Patrolling:
                if (canSeePlayer) State = EnemyState.Alert;
                break;
            case EnemyState.Alert:
                AlertState();
                break;
        }
    }

    private void AlertState()
    {
        if (!canSeePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, LastKnownLocation, MoveSpeed * Time.deltaTime);
        }
        else
        {            
            LastKnownLocation = Player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);
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
