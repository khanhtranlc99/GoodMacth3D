using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class SlotBird : MonoBehaviour
{
    public BirdMechanic birdMechanic;

    public Action action;
    public void ResetPosSlot(Transform paramPost)
    {
      
        DOTween.Kill(gameObject.transform);
        gameObject.transform.DOMove(paramPost.position, 0.2f).SetEase(Ease.OutQuad);
       
    }
}
