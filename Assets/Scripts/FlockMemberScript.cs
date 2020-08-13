using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMemberScript : MonoBehaviour
{
    public float speed = 0.5f;
    float rotateSpeed = 4.0f;
    Vector3 flockHeading;
    Vector3 flockPosition;
    float separation = 3.5f;
    bool turning = false;
    int index = 0;
    public bool isLeader = false;

    void Start()
    {
        StartValues();
        
    }
    
    public void StartValues()//Reset the starting speed and separation depending on what task the flock members are performong
    {
        if (FlockControllerScript.followPath)
        {
            separation = 0.0f;
            speed = Random.Range(3f, 6f);
        }
        else
        {
            separation = 3.5f;
            speed = Random.Range(1.0f, 3f);
        }

    }

    void Update()
    {
        //Decides if the member needs to turn around and if so, the speed is set to something more appropriate for having turned around
        if(Vector3.Distance(transform.position, Vector3.zero) >= FlockControllerScript.worldSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
        if (turning)
        {
            Vector3 heading = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), rotateSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else//Have the member update the behaviors on a slight random chance to give a bit of variation and better look
        {
            if (Random.Range(0, 5) < 1)
            {
                PerformBehaviors();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void PerformBehaviors()
    {
        GameObject[] otherMembers = FlockControllerScript.flock;

        Vector3 flockCenter = Vector3.zero;
        Vector3 avoidance = new Vector3(0.5f, 0.5f, 0.5f);
        float flockSpeed = 0.1f;
        //Set the target position based upon what functionality has been set in the Controller
        Vector3 targetPosition = Vector3.zero;
        if (FlockControllerScript.lazyFlight||FlockControllerScript.followLeader|| FlockControllerScript.feeding)
        {
            targetPosition = FlockControllerScript.targetPosition;
            if (isLeader)
            {
                targetPosition = FlockControllerScript.leaderTarget;
            }
        }
        else if (FlockControllerScript.followPath)
        {
            speed = 5f;
            targetPosition = FlockControllerScript.circlePath[index];
            if (Vector3.Distance(transform.position, targetPosition) <= 0.4f)//Update to next point if on a path
            {
                index++;
                if(index == FlockControllerScript.circlePath.Length)
                {
                    index = 0;
                }
            }
        }
        float distance;

        int flockSize = 0;
        foreach(GameObject m in otherMembers)
        {
            if(m != this.gameObject)//Get information from the other members of the flock
            {
                distance = Vector3.Distance(m.transform.position, this.transform.position);
                if(distance <= separation)//Update flock information for this member if the other member is within the correct distance
                {
                    flockCenter += m.transform.position;
                    flockSize++;
                    if(distance < 1.0f)//Avoid crowding
                    {
                        avoidance = avoidance + (this.transform.position - m.transform.position);
                    }

                    FlockMemberScript otherFlock = m.GetComponent<FlockMemberScript>();
                    flockSpeed = flockSpeed + otherFlock.speed;
                }
            }
        }

        if(flockSize > 0)//Average out speed amd position of the flock and set for the current member
        {
            flockCenter = flockCenter / flockSize + (targetPosition - this.transform.position);
            speed = flockSpeed / flockSize;

            Vector3 heading = (flockCenter + avoidance) - transform.position;
            if(heading != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(heading), rotateSpeed * Time.deltaTime);
            }
        }
    }
}
