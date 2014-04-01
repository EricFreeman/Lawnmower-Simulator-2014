using Assets.Scripts.Models;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<Map>();
        gameObject.GetComponent<Map>().LevelName = "TestLevel";
    }

    // Update is called once per frame
    void Update()
    {

    }
}