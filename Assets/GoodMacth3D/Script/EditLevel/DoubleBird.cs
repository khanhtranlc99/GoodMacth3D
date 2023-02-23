using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBird : MonoBehaviour
{
    public int idCowInData;
    public bool right;
    public bool stadingFront;
    public bool stadingBehind;
    public BirdMechanic birdInFront;
    public BirdMechanic birdInTheBack;
    [HideInInspector]  public Vector3 postFront;
    [HideInInspector] public Vector3 postInBack;
    public void Init()
    {
        birdInFront.idCowInData = idCowInData;
        birdInFront.standing = stadingFront;
        if (birdInTheBack != null)
        {
            birdInTheBack.idCowInData = idCowInData;
            birdInTheBack.right = right;
            birdInTheBack.standing  = stadingBehind;
        }
        birdInFront.right = right;
        SetTranform(birdInFront, birdInTheBack);
        birdInFront.Init();
    
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

