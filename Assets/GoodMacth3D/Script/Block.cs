using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    public int id;
    public int idElement;
    public bool wasLock;
    public AnimBird animBird;
    public GameObject postBird;
    public bool right;
    public void OnMouseDown()
    {
        if(!wasLock)
        {
            OnClick();
        }
       
    }

    public void Init()
    {
      
        var SpawnBird = Level.Instance;
        var CurrentScale = new Vector3(); 
        animBird = Instantiate(SpawnBird.GetAnimBird(id).animBird, right ? SpawnBird.leftPost.position : SpawnBird.rightPost.position, Quaternion.identity);
        animBird.SetAnim(animBird.FlY, true);
        if (right)
        {
            CurrentScale = new Vector3(-animBird.transform.localScale.x, animBird.transform.localScale.y, animBird.transform.localScale.z);
        }
        else
        {
            CurrentScale = animBird.transform.localScale;
        }

        animBird.transform.SetParent(this.transform);
        animBird.transform.localScale = CurrentScale;

        animBird.transform.DOMove(postBird.transform.position, 0.5f).OnComplete(
            delegate
            {
                animBird.transform.SetParent(postBird.transform);
                animBird.transform.localScale = CurrentScale;
                animBird.SetAnim(animBird.IDLE, true);
            }
         );
   
      
    }




    public void OnClick()
    {
        wasLock = true;
           var controler = Level.Instance;
        controler.SortBlocks(this);
        controler.lsLockDelete.Add(1);
        animBird.SetAnim(animBird.FlY, true);
        this.transform.DOMove(Level.Instance.GetPost(idElement).post.position, 0.6f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);

            }
            animBird.SetAnim(animBird.IDLE, true);
            controler.HandleDeleteBlocks(this);

        });
    }


}
