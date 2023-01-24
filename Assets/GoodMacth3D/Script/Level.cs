using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class Level : MonoBehaviour
{
    public static Level Instance;

    public List<int> tempLs;
    public List<int> tempLsIdBlock;
    public List<OrderOfBlock> lsBlockDelete;
    public List<int> lsLockDelete;
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
    public Block GetBlock(int id)
    {
        foreach(var item in lsBlock)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }
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
 
    public void Shuffle(Block block)
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
       
        SortIdElement();
        MoveBlock();
    }
    public void SortIdElement()
    {
        for (int i = 0; i < lsBlock.Count; i++)
        {
            lsBlock[i].idElement = i;
        }
    }

 
    public void MoveBlock()
    {
        for (int i = 0; i < lsBlock.Count; i++)
        {
            lsBlock[i].transform.DOMove(lsPost[i].post.position, 0.5f).OnComplete(delegate { SortIdElement(); });
        }

    }
    public void CheckDelete()
    {
        if(lsLockDelete.Count > 0)
        {
            return;
        }

         tempLs = new List<int>();
        tempLsIdBlock = new List<int>();
        lsIdAndNumb = new List<IdAndNumb>();
        lsBlockDelete = new List<OrderOfBlock>();
         foreach (var item in lsBlock)
        {
            tempLsIdBlock.Add(item.id);
        }
         tempLs = tempLsIdBlock.Distinct().ToList();
        for(int i = 0; i < tempLs.Count; i++)
        {
            lsIdAndNumb.Add(new IdAndNumb() { id = tempLs[i], numb = 1 });
        }

        for (int i = 0; i < tempLsIdBlock.Count - 1; i++)
        {
            for (int j = i + 1; j < tempLsIdBlock.Count; j++)
            {
                if (tempLsIdBlock.ElementAt(i) == tempLsIdBlock.ElementAt(j))
                {
                   foreach(var item in lsIdAndNumb)
                    {
                        if(item.id == tempLsIdBlock[i])
                        {
                            item.numb += 1;

                            if (item.numb >= 3)
                            {
                                item.numb = 0;
                               
                                for(int k = lsBlock.Count -1; k >=0; k--)
                                {
                                    if (lsBlock[k].id == item.id)
                                    {
                                        Destroy(lsBlock[k].gameObject);
                                        lsBlock.RemoveAt(k);
                                        MoveBlock();
                                    }
                                }
                               
                            }

                        }
                    }
                    break;
                }
            }
        }
     
        //HandleDelete();
    }

    public void HandleDelete()
    {
        if(lsBlockDelete.Count >= 3)
        {
            

            for (int i = 0; i < 3; i++)
            {
               
                    Destroy(lsBlockDelete[i].block.gameObject);
                    lsBlock.RemoveAt(lsBlockDelete[i].order);
                    lsBlockDelete.RemoveAt(i);
                
            

            }
            MoveBlock();
        }
      
    }


    public void TestDelete(Block paramBlock)
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
                    for(int i = temp.lsBlock.Count -1; i >= 0; i-- )
                    {
                        Destroy(temp.lsBlock[i].gameObject);
                        temp.lsBlock.RemoveAt(i);
                    }
                    temp.numb = 0;
                  
                }
            }
      
            for (int i = lsBlock.Count -1; i >= 0; i--)
            {
                if (lsBlock[i] == null)
                {
                    lsBlock.RemoveAt(i);
                }
            }
            MoveBlock();
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
[System.Serializable]
public class OrderOfBlock
{
    public int order;
    public Block block;
}