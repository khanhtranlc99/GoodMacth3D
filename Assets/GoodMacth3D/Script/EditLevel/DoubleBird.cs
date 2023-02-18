using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBird : MonoBehaviour
{
    public int idCowInData;
    public bool right;
    public BirdMechanic birdInFront;
    public BirdMechanic birdInTheBack;
    public Vector3 postFront;
    public Vector3 postInBack;
    public void Init()
    {
        birdInFront.idCowInData = idCowInData;
        birdInTheBack.idCowInData = idCowInData;
        birdInFront.right = right;
        birdInTheBack.right = right;
        birdInFront.Init();
        SetTranform(birdInFront, birdInTheBack);
    }
    public void SetTranform(BirdMechanic front, BirdMechanic back)
    {
        postFront = front.transform.position;
        postInBack = back.transform.position;
    }
}

