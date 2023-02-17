using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class SlotBird : MonoBehaviour
{
    public BirdMechanic birdMechanic;
    public Post post;
    public Action action;
    public void ResetPosSlot(Post paramPost)
    {
      
        DOTween.Kill(gameObject.transform);
        if (post == null)
        {
            post = paramPost;
        
            gameObject.transform.transform.DOMove(post.midPost.position, 0.5f).OnComplete(delegate
            {
                gameObject.transform.transform.DOMove(post.finalPost.position, 0.2f);
            }
            
       );
        }
        else
        {
            if (post != paramPost)
            {
                post = paramPost;
                RotateBird(post.transform);
                birdMechanic.animBird.SetAnim(birdMechanic.animBird.FlY,true);
                gameObject.transform.transform.DOMove(post.midPost.position, 0.5f).OnComplete(delegate
                {
                    gameObject.transform.transform.DOMove(post.finalPost.position, 0.2f);
                    birdMechanic.animBird.SetAnim(birdMechanic.animBird.IDLE, true);
                }
               );
                Debug.LogError("vi tri moi");
            }
            else
            {
                gameObject.transform.transform.DOMove(post.finalPost.position, 0.2f);
                birdMechanic.animBird.SetAnim(birdMechanic.animBird.IDLE, true);
                Debug.LogError("giong vi tri cu");
            }
        }
        //gameObject.transform.DOMove(paramPost.position, 0.7f).SetEase(Ease.OutQuad);
        
    }

    private void RotateBird( Transform paramTranform)
    {
        var tempLocalScaleBird = birdMechanic.animBird.transform.localScale;
        var tempLocalScaleBirdBeforAbs = Math.Abs(tempLocalScaleBird.x);
        Debug.LogError(tempLocalScaleBirdBeforAbs);
        birdMechanic.animBird.transform.localScale = new Vector3(tempLocalScaleBirdBeforAbs, tempLocalScaleBird.y, tempLocalScaleBird.z);


        if (paramTranform.transform.position.x < this.transform.position.x)
        {

            birdMechanic.animBird.transform.localScale = new Vector3(-tempLocalScaleBirdBeforAbs, tempLocalScaleBird.y, tempLocalScaleBird.z);

        }

    }
}

