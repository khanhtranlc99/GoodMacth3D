using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
public class AnimBird : MonoBehaviour
{
    public string IDLE = "GAMEPLAY/idle";
    public string FlY = "GAMEPLAY/fly";
    public string JUMP = "DANCE/WIN";

    public SkeletonGraphic body;
    public Color blackColor;

    public void SetColor(bool offColor)
    {
        if(offColor)
        {
            body.color = blackColor;
        }
        else
        {
            body.color = new Color32(255, 255, 255, 255);
        }
    }
    public void SetAnim(string param, bool loop, Action callBack = null)
    {
        body.SetAnimation(param, loop, delegate {

            if (callBack != null)
            {
                callBack?.Invoke();
            }
                
          }) ;
    }
 
}
