using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset = new Vector3(0, 1, -10);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset+new Vector3(player.transform.position.x,0,player.transform.position.z-1);
    }
}
