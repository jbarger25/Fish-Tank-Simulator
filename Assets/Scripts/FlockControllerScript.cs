using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockControllerScript : MonoBehaviour
{

    public GameObject memberPrefab;
    public GameObject foodPrefab;
    GameObject food;
    [Range(5, 100)]
    static int flockSize = 100;
    public static GameObject[] flock = new GameObject[flockSize];
    public static float worldSize = 20f;
    public static Vector3 targetPosition = Vector3.zero;
    public static Vector3 leaderTarget = Vector3.zero;
    public static Vector3[] circlePath = { new Vector3(0, 0, 8f), new Vector3(-4f, 0, 6f), new Vector3(-8f, 0, 0), new Vector3(-4f, 0, -6f),
                                           new Vector3(0, 0, -8f), new Vector3(4f, 0, -6f), new Vector3(8f, 0, 0), new Vector3(4f, 0, 6f) }; 
    public static bool lazyFlight = true;
    public static bool followPath = false;
    public static bool followLeader = true;
    public static bool feeding = false;
    Color colorholder;

    void Start()//Instantiate the flock members and set other variables relevant to different functionalities such as the leader member and the default color
    {
        for(int i = 0; i <flockSize; i++)
        {
            Vector3 position = new Vector3(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize));

            flock[i] = (GameObject)Instantiate(memberPrefab, position, Quaternion.identity);
            flock[i].GetComponent<FlockMemberScript>().isLeader = false;
            flock[i].GetComponentInChildren<MeshRenderer>().materials[0] = flock[i].GetComponentInChildren<MeshRenderer>().materials[0];
        }
        flock[0].GetComponent<FlockMemberScript>().isLeader = true;
        colorholder = flock[0].GetComponentInChildren<MeshRenderer>().material.color;

    }


    void Update()//Check to see what functionality has been set for the flock and set the targets for the members accordingly
    {
        if (lazyFlight) {
            if (Random.Range(0, 10000) < 50)
            {
                targetPosition = new Vector3(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize));
            }
            
        }
        if (followPath)
        {
            targetPosition = circlePath[0];
        }
        if (followLeader)
        {
            targetPosition = flock[0].transform.position;
            if (Random.Range(0, 10000) < 50)
            {
                leaderTarget = new Vector3(Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize), Random.Range(-worldSize, worldSize));
            }
        }
        if (feeding)
        {
            targetPosition = food.transform.position;
        }

    }

    public void SetFollowLeader()//For use with UI button to set the flock to follow a leader. Identifies the leader by changing color and resets starting values
    {
        lazyFlight = false;
        followPath = false;
        followLeader = true;
        flock[0].GetComponentInChildren<MeshRenderer>().materials[0].color = Color.white;
        for (int i = 0; i < flockSize; i++)
        {
            flock[i].GetComponent<FlockMemberScript>().StartValues();
        }
    }
    public void SetLazyFlight()//For use with UI button to set the flock to target random points. De-Identifies the leader by reseting color and resets starting values
    {
        lazyFlight = true;
        followPath = false;
        followLeader = false;
        flock[0].GetComponentInChildren<MeshRenderer>().materials[0].color = colorholder;
        for (int i = 0; i < flockSize; i++)
        {
            flock[i].GetComponent<FlockMemberScript>().StartValues();
        }
    }

    public void SetCirclePath()//For use with UI button to set the flock to follow a set of points. De-Identifies the leader by reseting color and resets starting values
    {
        lazyFlight = false;
        followPath = true;
        followLeader = false;
        flock[0].GetComponentInChildren<MeshRenderer>().materials[0].color = colorholder;
        for (int i = 0; i < flockSize; i++)
        {
            flock[i].GetComponent<FlockMemberScript>().StartValues();
        }
    }
    public void Feed()//For use with UI button to set the flock to target a GameObject. Resets start values, but is independent of other functionalities. Can be called without resetting the others
    {
        feeding = true;

        food = Instantiate(foodPrefab, new Vector3(Random.Range(-(worldSize-2), (worldSize - 2)), (worldSize - 2), Random.Range(-(worldSize - 2), (worldSize - 2))), Quaternion.identity);
        for (int i = 0; i < flockSize; i++)
        {
            flock[i].GetComponent<FlockMemberScript>().StartValues();
        }
    }

    public void Quit()//Quit the game
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = (false);
#else
        Application.Quit();
#endif
    }
}
