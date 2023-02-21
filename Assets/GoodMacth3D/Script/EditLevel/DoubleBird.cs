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
        if(birdInTheBack != null)
        {
            birdInTheBack.idCowInData = idCowInData;
            birdInTheBack.right = right;
        }
        birdInFront.right = right;
        birdInFront.Init();
        SetTranform(birdInFront, birdInTheBack);
    }
    public void SetTranform(BirdMechanic front, BirdMechanic back)
    {
        postFront = front.transform.position;
        if(back != null)
        {
            postInBack = back.transform.position;
        }
    
    }
}

