using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    public int id;
    public int idElement;
    public bool wasLock;

    public Block unlockBlock;
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
        var controler = Level.Instance.levelLogic;
        controler.HandleEndGame(this); 
        controler.SortBlocks(this);
        controler.lsLockDelete.Add(1);
        this.transform.DOMove(controler.GetPost(idElement).post.position, 0.2f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);
            }       
            controler.HandleDeleteBlocks(this);
          
        });
        UnlockClickBlockBehide();
    }

    public void UnlockClick()
    {
        wasLock = false;
    }
    public void LockClick()
    {
        wasLock = true;
    }
    public void UnlockClickBlockBehide()
    {
        if(unlockBlock != null)
        {
            unlockBlock.UnlockClick();
            unlockBlock.transform.DOMove(this.transform.position, 0.2f);
        }
    }
    private void OnDestroy()
    {
        if(Level.Instance.levelSpawn.levelData.numBlock > 0)
        {
            Level.Instance.levelSpawn.levelData.numBlock -= 1;
        }
    }
}
