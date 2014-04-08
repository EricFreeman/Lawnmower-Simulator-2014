using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Player;
    public float FieldOfView = 60f;
    public float ViewDistance = 5.5f;
    public float MoveSpeed = 2f;
    public float TurnSpeed = 6f;

    public EnemyState State = EnemyState.Patrolling;
    private Vector3 LastKnownLocation;

    private bool canSeePlayer
    {
        get
        {
            Vector3 rayDirection = Player.transform.position - transform.position;

            // Detect if player is within the field of view
            if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit) && Vector3.Distance(transform.position, Player.transform.position) < ViewDistance)
                    if (hit.transform.tag == "Player")
                        return true;
            }

            return false;
        }
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        switch (State)
        {
            case EnemyState.Idle:
                if (canSeePlayer) State = EnemyState.Detect;
                break;
            case EnemyState.Patrolling:
                if (canSeePlayer) State = EnemyState.Detect;
                break;
            case EnemyState.Searching:
                SearchState();
                break;
            case EnemyState.Detect:
                AlertState();
                break;
        }
    }

    private bool searchDir;
    private int searchAmount;

    private void SearchState()
    {
        if (canSeePlayer)
        {
            searchAmount = 0;
            State = EnemyState.Detect;
        }

        var stopAmount = Math.Abs(searchAmount - transform.rotation.eulerAngles.y);
        if (stopAmount < 3 || (stopAmount - 360 < 3 && stopAmount - 360 > 0))
        {
            searchDir = !searchDir;
            searchAmount = (int)transform.rotation.eulerAngles.y + (120 * (searchDir ? 1 : -1));
        }

        transform.Rotate(0, 1 * (searchDir ? 1 : -1), 0);
    }

    private void AlertState()
    {
        if (!canSeePlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, LastKnownLocation, MoveSpeed * Time.deltaTime);

            if(transform.position == LastKnownLocation)
                State = EnemyState.Searching;
        }
        else
        {
            LastKnownLocation = Player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, MoveSpeed * Time.deltaTime);

            var targetRotation = Quaternion.LookRotation(Player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }
    }
}

public enum EnemyState
{
    Idle,
    Patrolling,
    Searching,
    Detect,
    Dead
}
