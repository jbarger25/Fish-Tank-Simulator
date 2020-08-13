using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    void Update()
    {
       if(transform.position.y < -15)//Destroy the food object once it has fallen a certain distance in the tank
        {
            FlockControllerScript.feeding = false;
            Destroy(gameObject);
        }
    }
}
