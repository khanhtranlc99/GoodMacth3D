using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBird : MonoBehaviour
{
    public int idCowInData;
    public BirdMechanic birdInFront;
    public BirdMechanic birdInTheBack;
    public void Init()
    {
        birdInFront.idCowInData = idCowInData;
        birdInTheBack.idCowInData = idCowInData;
        birdInFront.Init();
    }
}

