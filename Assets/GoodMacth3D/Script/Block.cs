using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    public int id;
    public int idElement;
    public void OnMouseDown()
    {
        OnClick();
    }
    public void OnClick()
    {
        var controler = Level.Instance;
        controler.SortBlocks(this);
        controler.lsLockDelete.Add(1);
        this.transform.DOMove(Level.Instance.GetPost(idElement).post.position,  0.5f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);

            }
            controler.HandleDeleteBlocks(this) ;
            controler.SortIdElementBlocks();
        });
    }


}
