using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class Level : MonoBehaviour
{
    public static Level Instance;
    public List<IdAndNumb> lsIdAndNumb;
    public IdAndNumb GetIdAndNumb(int id)
    {
        foreach (var item in lsIdAndNumb)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public List<Block> lsBlock;
    public List<Post> lsPost;
    public Post GetPost(int id)
    {
        foreach (var item in lsPost)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public void Awake()
    {
        Instance = this;
    }

    public void SortBlocks(Block block)
    {
        if(lsBlock.Count <= 0)
        {
            lsBlock.Add(block);
        }
        else
        {
            for(int i = 0; i < lsBlock.Count; i ++)
            {
                if(lsBlock[i].id == block.id)
                {
                    lsBlock.Insert(i+1, block);                 
                    break;
                }
                else
                {
                    lsBlock.Add(block);       
                    break; 
                }             
            }      
        }     
        SortIdElementBlocks();
        MoveBlocks();
    }
    public void SortIdElementBlocks()
    {
        for (int i = 0; i < lsBlock.Count; i++)
        {
            lsBlock[i].idElement = i;
        }
    }
    public void MoveBlocks()
    {
        for (int i = 0; i < lsBlock.Count; i++)
        {
            lsBlock[i].transform.DOMove(lsPost[i].post.position, 0.5f).OnComplete(delegate { SortIdElementBlocks(); });
        }
    }
    public void HandleDeleteBlocks(Block paramBlock)
    {
        var temp = GetIdAndNumb(paramBlock.id);
      if (temp != null)
        {      
            if (temp.lsBlock.Count < 3)
            {
                temp.numb += 1;
                temp.lsBlock.Add(paramBlock);
                if(temp.numb == 3)
                {
                    for (int i = temp.lsBlock.Count - 1; i >= 0; i--)
                    {
                        lsBlock.Remove(temp.lsBlock[i]);
                      
                    }
                    for (int i = temp.lsBlock.Count - 1; i >= 0; i--)
                    {

                        Destroy(temp.lsBlock[i].gameObject);
                        temp.lsBlock.RemoveAt(i);
                    }
                    temp.numb = 0;               
                }
            }      
            MoveBlocks();
        }
    }

}
[System.Serializable]
public class IdAndNumb
{
    public int id;
    public int numb;
    public List<Block> lsBlock;
    public IdAndNumb ()
        {
        lsBlock = new List<Block>();
        }
}
