using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        controler.HandleEndGame(this); 
        controler.SortBlocks(this);
        controler.lsLockDelete.Add(1);
        this.transform.DOMove(Level.Instance.GetPost(idElement).post.position, 0.35f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);
            }       
            controler.HandleDeleteBlocks(this);
          
        });
    }
}
