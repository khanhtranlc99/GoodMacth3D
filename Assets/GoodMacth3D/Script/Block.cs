using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class Block : MonoBehaviour
{
    public int id;
    public int idElement;
    public bool wasLock;
    public void OnMouseDown()
    {
        if(!wasLock)
        {
            OnClick();
        }
       
    }
    public void OnClick()
    {
        wasLock = true;
           var controler = Level.Instance;
        controler.SortBlocks(this);
        controler.lsLockDelete.Add(1);
        Fly(delegate {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);

            }
            controler.HandleDeleteBlocks(this);
        });
    }
    public void Fly(Action callback)
    {
      
        var controler = Level.Instance;
        //controler.lsLockDelete.Add(1);
        this.transform.DOMove(controler.GetPost(idElement).post.position, 1).OnComplete(delegate
        {

            callback?.Invoke();
     

        });
    }


}
