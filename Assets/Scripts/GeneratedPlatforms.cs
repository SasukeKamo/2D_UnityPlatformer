using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedPlatforms : MonoBehaviour
{
    [SerializeField] public GameObject platformPrefab;
    [SerializeField] private float speed = 1.0f;
    const int PLATFORMS_NUM = 2;
    GameObject[] platforms;
    Vector3[] positions;
    Vector3[] DstPositions;
    float radius = 0.2f;
    float elapsedTime = 0f;

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        DstPositions = new Vector3[PLATFORMS_NUM];
        for (int i=0;i<PLATFORMS_NUM;i++)
        {
            positions[i].x = this.gameObject.transform.position.x-i*0.2f;
            positions[i].y = this.gameObject.transform.position.y+i*0.6f;
            positions[i].z = this.gameObject.transform.position.z;

            //DstPositions[i].x = this.gameObject.transform.position.x - (i * 0.2f) - 0.3f;
            //DstPositions[i].y = this.gameObject.transform.position.y + (i * 0.6f) - 0.3f;
           // DstPositions[i].z = this.gameObject.transform.position.z - 0.3f;

            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float angle = elapsedTime * speed;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);       
        for (int i=0;i<PLATFORMS_NUM;i++)
        {
            //platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position, DstPositions[i], speed * Time.deltaTime);
            if(i%2==0)
            {
                platforms[i].transform.position = new Vector3(positions[i].x + x, positions[i].y + y, positions[i].z);
            }
            else
            {
                platforms[i].transform.position = new Vector3(positions[i].x - x, positions[i].y - y, positions[i].z);
            }
        }
    }
}
