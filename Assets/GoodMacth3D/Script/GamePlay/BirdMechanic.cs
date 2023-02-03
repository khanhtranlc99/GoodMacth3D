using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
public class BirdMechanic : MonoBehaviour
{
    public int id;
    public int idElement;
    public bool wasLock;
    public BirdMechanic unlockBlock;

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
        controler.SortBird(this);
        controler.lsLockDelete.Add(1);
        this.transform.DOMove(controler.GetPost(idElement).post.position, 0.2f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);
            }       
            controler.HandleDeleteBirds(this);
          
        });
        UnlockClickBlockBehide();
        Debug.LogError("OnClick");
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
            unlockBlock.transform.DOJump(this.transform.position, 3, 1, 0.5f);
        }
    }
    private void OnDestroy()
    {
        //if(Level.Instance.levelSpawn.levelData.numBlock > 0)
        //{
        //    Level.Instance.levelSpawn.levelData.numBlock -= 1;
        //}
    }
}
