using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class Level : MonoBehaviour
{
    public static Level Instance;


    public LevelLogic levelLogic;
    public LevelSpawn levelSpawn;
    public void Awake()
    {
        Instance = this;

        levelSpawn.Init();
        levelLogic.Init(this) ;
    }
  


        

}
