using System;
using UnityEngine;

public class RepeaterController : MonoBehaviour {
    int nbrGroundBits, nbrCloudBits, nbrBackgroundBits, nbrSkyBits;
    float groundBitLength, cloudBitLength, backgroundBitLength, skyBitLength;
    float randomValue;
    private const float horizontalShift = 45f; 
    private double startPositionTop = 19.88;
    private double startPositionBottom  = -6.39;
    private double startPositionHeart = 6.18;
 
    void Start() {
        //startPositionTop = 7.410321;
        //startPositionBottom = -18.85968;

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Ground");
        nbrGroundBits = temp.Length;
        groundBitLength = GameObject.FindGameObjectWithTag("Ground").transform.lossyScale.x;

        temp = GameObject.FindGameObjectsWithTag("Cloud");
        nbrCloudBits = temp.Length;
        cloudBitLength = GameObject.FindGameObjectWithTag("Cloud").transform.lossyScale.x;

        temp = GameObject.FindGameObjectsWithTag("Background");
        nbrBackgroundBits = temp.Length; 
        backgroundBitLength = GameObject.FindGameObjectWithTag("Background").transform.lossyScale.x; 
    
        temp = GameObject.FindGameObjectsWithTag("Sky");
        nbrSkyBits = temp.Length; 
        skyBitLength = GameObject.FindGameObjectWithTag("Sky").transform.lossyScale.x; 
    }
    void Update(){
        randomValue = UnityEngine.Random.Range(-4.5f, 4.5f); //placed in update() to ensure the same value is used for top and bottom tower despite order of collison
    }
     
    private void OnTriggerEnter2D(Collider2D collision) {
        Vector3 newPosition = collision.transform.position;
        
        //when repeater hits objects, move bits forward
        if (collision.tag == "Ground") {       newPosition.x += groundBitLength * nbrGroundBits; }
        if (collision.tag == "Cloud") {       newPosition.x += cloudBitLength * nbrCloudBits; }
        if (collision.tag == "Background") {   newPosition.x += backgroundBitLength * nbrBackgroundBits; }
        if (collision.tag == "Sky") {          newPosition.x += skyBitLength * nbrSkyBits; }
        
        if (collision.tag == "LifePickup") {    //in case the player missed to pick up the life
            Debug.Log("heartposition:" + newPosition.y);
            newPosition.x += 60; 
            newPosition.y = (float)startPositionHeart + randomValue;
        }

        if (collision.tag == ("TopTower")) {
            newPosition.x += horizontalShift;
            newPosition.y = (float)startPositionTop + randomValue;
        }
        if (collision.CompareTag("BottomTower")) {
            newPosition.x += horizontalShift;
            newPosition.y = (float)startPositionBottom + randomValue;
        }

        collision.transform.position = newPosition;
        
    }

}
