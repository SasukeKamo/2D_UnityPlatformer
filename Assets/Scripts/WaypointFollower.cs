using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    int currentWaypoint = 0;
    [SerializeField] private float speed = 1.0f;
    bool getBack = false;

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            float distance = Vector2.Distance(this.transform.position, waypoints[currentWaypoint].transform.position);
            //Debug.Log(distance);
            if (distance < 0.1f && !getBack)
            {
                currentWaypoint = (currentWaypoint + 1);
                if (currentWaypoint == 2)
                {
                    getBack = true;
                }
            }
            else if (distance < 0.1 && getBack)
            {
                currentWaypoint = (currentWaypoint - 1);
                if (currentWaypoint == 0)
                {
                    getBack = false;
                }
            }

            this.transform.position = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
        }
    }

}
