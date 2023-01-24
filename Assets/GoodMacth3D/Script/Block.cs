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
        this.transform.DOMove(Level.Instance.GetPost(idElement).post.position,  0.5f).OnComplete(delegate
        { 
            controler.HandleDeleteBlocks(this) ;
            controler.SortIdElementBlocks();
        });
    }


}
