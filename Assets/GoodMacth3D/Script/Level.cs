using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System.Linq;


public class Level : MonoBehaviour
{
    public static Level Instance;
    public List<Block> lsBlock;
    public List<int> tesst;
    public List<int> lsLockDelete;
    public List<IdAndNumb> lsIdAndNumb;
    public GameObject losePanel;

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
    public void Start()
    {
        //Input.multiTouchEnabled = false;
    }

    public void SortBlocks(Block block)
    {
        if(lsBlock.Count <= 0)
        {
            lsBlock.Add(block);
            tesst.Add(block.id);
        }
        else
        {
            bool same = false;
            int order = 0;
            for (int i = 0; i < lsBlock.Count; i ++)
            {
                if(lsBlock[i].id == block.id)
                {              
                    same = true;
                    order = i;
                }                   
            }      
            if(same)
            {
                lsBlock.Insert(order + 1, block);
                tesst.Insert(order + 1, block.id);
            }
            else
            {
                lsBlock.Add(block);
                tesst.Add(block.id);
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
            lsBlock[i].transform.DOMove(lsPost[i].post.position, 0.2f).OnComplete(delegate { SortIdElementBlocks(); DeleteBlocks(); });
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
                if (lsLockDelete.Count > 0)
                {
                    return;

                }
                DeleteBlocks();
              
            }      
        
        }
    }


    private void DeleteBlocks()
    {
        for(int i = lsIdAndNumb.Count -1; i >=0 ; i --)
        {
            if(lsIdAndNumb[i].numb == 3)
            {
                for(int j = lsIdAndNumb[i].lsBlock.Count - 1 ; j >= 0; j --)
                {

                    lsBlock.RemoveAt(lsIdAndNumb[i].lsBlock[j].idElement);
                    tesst.RemoveAt(lsIdAndNumb[i].lsBlock[j].idElement);
                }
                for (int j = lsIdAndNumb[i].lsBlock.Count - 1; j >= 0; j--)
                {

                    Destroy(lsIdAndNumb[i].lsBlock[j].gameObject);
                    lsIdAndNumb[i].lsBlock.RemoveAt(j);
                }
                lsIdAndNumb[i].numb = 0;
                SortIdElementBlocks();
                MoveBlocks();
              
            }

        }
    }

    public void HandleEndGame(Block paramBlock)
    {
        bool firtCondition = false;
        bool secondCondition = false;
        if (lsBlock.Count >= 6)
        {
            var temp = GetIdAndNumb(paramBlock.id);
            if (temp != null)
            {
                if (temp.numb == 2)
                {
                    firtCondition = true;
                  
                }
            }
            for (int i = lsIdAndNumb.Count - 1; i >= 0; i--)
            {
                if (lsIdAndNumb[i].numb == 2)
                {
                    secondCondition = true;
                   
                }

            }
            Debug.LogError("firtCondition " + firtCondition);
            Debug.LogError("secondCondition " + secondCondition);

            if (firtCondition)
            {
                return;
            }

            if (!firtCondition && secondCondition)
            {

                return;
            }

            //if (firtCondition == true && secondCondition == false)
            //{
            //    losePanel.SetActive(true);
            //}

            losePanel.SetActive(true);
            
        }
     
    }
    public void Test()
    {
        if (lsLockDelete.Count > 0)
        {
            return;

        }
        if (lsBlock.Count >= 7)
        {
            losePanel.SetActive(true);
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
