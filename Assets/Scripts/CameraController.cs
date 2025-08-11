using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Transform playerPosition;
    private float sidLedsOffset;
    private float hojdLedsOffset;

    private void Start() {
        GameObject player_go = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player_go.transform;
        sidLedsOffset = transform.position.x - playerPosition.position.x;
        //hojdLedsOffset = transform.position.y - playerPosition.position.y;
    }

    private void Update() {
        Vector3 uppdateradCameraPosition = transform.position;
        
        uppdateradCameraPosition.x = playerPosition.position.x + sidLedsOffset;
        //uppdateradCameraPosition.y = playerPosition.position.y + hojdLedsOffset;
        
        transform.position = uppdateradCameraPosition;
    }
}
