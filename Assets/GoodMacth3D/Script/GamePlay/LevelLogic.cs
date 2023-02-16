using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class LevelLogic : MonoBehaviour
{
    public List<BirdMechanic> lsBirdEndGame;
    public List<BirdMechanic> lsBird;
    public List<int> tesst;
    public List<int> lsLockDelete;
    public List<IdAndNumb> lsIdAndNumb;
    public GameObject losePanel;
    public GameObject winPanel;
    private Level level;
    public List<IdAndNumbBirdDuplicate> lsCount;
    public List<int> lsBirdTemp;
    public Transform parentSlotBird;
    public SlotBird slotBird;
    public List<SlotBird> lsSlotBird = new List<SlotBird>();
    public List<SlotBird> listCheckUndo_ItemTileSlots = new List<SlotBird>();
    public void Init(Level param)
    {
        level = param;
        SetUp();
    }


    public IdAndNumb GetIdAndNumb(int id)
    {
        foreach (var item in lsIdAndNumb)
        {
            if (item.id == id)
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
   
    public void SetUp()
    {
        //for(int i = 0; i < level.levelSpawn.levelData.idBlock.Count; i ++)
        //{
        //    lsIdAndNumb.Add(new IdAndNumb() { id = level.levelSpawn.levelData.idBlock[i], numb = 0 });
        //}
    }
   

    public void AddBirdToListSlot(BirdMechanic bird)
    {
        int indexNewSlot = GetPostOfSlot(bird);


        var tempSlot = SimplePool2.Spawn(slotBird, GetPost(indexNewSlot).finalPost.transform.position, Quaternion.identity);

        tempSlot.birdMechanic = bird;
        tempSlot.transform.parent = parentSlotBird;
        bird.transform.parent = tempSlot.transform;

        bird.Fly(tempSlot.transform);
  
        listCheckUndo_ItemTileSlots.Add(tempSlot);
        lsSlotBird.Insert(indexNewSlot, tempSlot);

        StartCoroutine(SetListSlot_ResetPosition_Now());

        Debug.LogError(GetPost(indexNewSlot).gameObject.name);
    }

    public int GetPostOfSlot(BirdMechanic bird)
    {
        int indexSlot = lsSlotBird.Count;
        for (int i = lsSlotBird.Count - 1; i >= 0; i--)
        {
            if (lsSlotBird[i].birdMechanic.id == bird.id)
            {
                return i + 1;
            }
        }
        return indexSlot;
    }

    public IEnumerator SetListSlot_ResetPosition_Now()
    {
        yield return new WaitForSeconds(0.01f);
        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            lsSlotBird[i].ResetPosSlot(GetPost(i).finalPost);
        }

    }









    public void MoveSlotBird()
    {
        //if(lsSlotBird.Count > 0)
        //{
        //    for (int i = 0; i < lsSlotBird.Count; i++)
        //    {
        //        var index = i;
        //        lsSlotBird[index].MoveSlot(GetPost(lsSlotBird[index].idElement).finalPost, 1, delegate {

                   
        //        });
        //    }

        //}

    }

    public void SortIdElementBirds()
    {
        //for (int i = 0; i < lsBird.Count; i++)
        //{
        //    lsBird[i].idElement = i;
        //}
        //for (int i = 0; i < lsBird.Count; i++)
        //{
        //    if (lsBird[i].slotBird != null)
        //    {
        //        lsBird[i].slotBird.idElement = lsBird[i].idElement;
        //    }
           
        //}
    }
    public void MoveBlocks(BirdMechanic birdMechanic =null)
    {
     
    }
    public void HandleDeleteBirds(BirdMechanic paramBlock)
    {
        var temp = GetIdAndNumb(paramBlock.id);
        if (temp != null)
        {
            if (temp.lsBird.Count < 3)
            {
                temp.numb += 1;
                temp.lsBird.Add(paramBlock);
                temp.lsAnimBird.Add(paramBlock.animBird);
                if (lsLockDelete.Count > 0)
                {
                    return;
                }
               
                DeleteBirds();
            }
        }
    }


    private void DeleteBirds()
    {
    
        for (int i = lsIdAndNumb.Count - 1; i >= 0; i--)
        {
            if (lsIdAndNumb[i].numb == 3)
            {
                for (int j = lsIdAndNumb[i].lsBird.Count - 1; j >= 0; j--)
                {
                    lsBird.RemoveAt(lsIdAndNumb[i].lsBird[j].idElement);
                    tesst.RemoveAt(lsIdAndNumb[i].lsBird[j].idElement);
                }
               

                for (int j = lsIdAndNumb[i].lsBird.Count - 1; j >= 0; j--)
                {
                   lsIdAndNumb[i].lsBird[j].gameObject.SetActive(false);
                    //lsIdAndNumb[i].lsBird[j].TestSpawn();
                  //  Destroy(lsIdAndNumb[i].lsBlock[j].gameObject);
                    level.levelSpawn.levelData2.sumBird -= 1;
                    lsIdAndNumb[i].lsBird.RemoveAt(j);
                }


              
                lsIdAndNumb[i].numb = 0;
         
                SortIdElementBirds();
                //MoveBlocks();
          
            }
  

        }
      
        HandleWin();

      

    }
    private void TestEffect()
    {
        for (int i = lsIdAndNumb.Count - 1; i >= 0; i--)
        {
            if (lsIdAndNumb[i].numb == 3)
            {
              
                lsIdAndNumb[i].HandleOffBird(this.transform);
            }
           
        }
    }

    public void HandleCheckLose(BirdMechanic paramBlock)
    {
        if (lsBird.Count >= 6)
        {
            lsBirdEndGame.Add(paramBlock);
            if (lsBirdEndGame.Count <= 1)
            {
                var temp = GetIdAndNumb(lsBirdEndGame[0].id);
                if (temp.numb >= 2)
                {
                    Debug.LogError("numb " );
                    return;
                }

                if (SuportCheckLoseOneBird)
                {
                    return;
                }
                if(SuportCheckLoseMethodOneBird(lsBirdEndGame[0]))
                {
                    return;
                }

                Debug.LogError("One ");
            }
            else
            {
                for (int i = lsBirdEndGame.Count - 1; i >= 0; i--)
                {
                    var temp = GetIdAndNumb(lsBirdEndGame[i].id);
        
                    if (temp.numb >= 2)
                    {
                       return;
                    }
                }

                var conditionHas1Bird = false;
                var conditionHas2BirdFlying = false;
                var birdCondition = new BirdMechanic();
                for (int i = 0; i < lsBirdEndGame.Count - 1; i++)
                {
                    for (int j = i + 1; j < lsBirdEndGame.Count; j++)
                    {
                        if (lsBirdEndGame.ElementAt(i) == lsBirdEndGame.ElementAt(j))
                        {
                            conditionHas2BirdFlying = true;
                            birdCondition = lsBirdEndGame[i];
                        }
                       
                    }
                }
                if (birdCondition != null)
                {
                    var tempBirdCount = GetIdAndNumb(birdCondition.id);
                    if (tempBirdCount.numb == 1)
                    {
                        conditionHas1Bird = true;
                    }
                    if (conditionHas1Bird && conditionHas2BirdFlying)
                    {
                        return;
                    }
                    Debug.LogError("1111111111");
                }
                else
                {
                    for (int i = lsBirdEndGame.Count - 1; i >= 0; i--)
                    {
                        if(SuportCheckLoseMethodOneBird(lsBirdEndGame[i]))
                        {
                            return;
                        }
                    }
                    Debug.LogError("2222222222");
                }
              

                Debug.LogError("Double");
            }
            losePanel.SetActive(true);
 
        }
        else
        {
            lsBirdEndGame.Clear();
        }

        
    }

    private bool SuportCheckLoseOneBird
    {
        get
        {
            lsCount = new List<IdAndNumbBirdDuplicate>();
            lsBirdTemp = new List<int>();

            foreach (var item in lsBird)
            {
                lsBirdTemp.Add(item.id);
            }

            var ienum = lsBirdTemp.Distinct();
            lsBirdTemp = ienum.ToList<int>();

            foreach (var item in lsBirdTemp)
            {
                lsCount.Add(new IdAndNumbBirdDuplicate() { id = item });
            }


            for (int i = lsBird.Count -1; i >=0 ; i--)
            {
                foreach (var item in lsCount)
                {
                    if (item.id == lsBird[i].id)
                    {
                        item.numb += 1;
                    }
                }
            }

            foreach (var item in lsCount)
            {
                if (item.numb >= 3)
                {
                    return true;
                }
            }

            return false;
        }
    

    }
    private bool SuportCheckLoseMethodOneBird(BirdMechanic birdMechanic)
    {
        var count = 0;
        for (int i = 0; i < lsBird.Count; i++)
        {
            if(lsBird[i].id == birdMechanic.id)
            {
                count += 1;
            }
        }
     
        if (count >= 2)
        {
            return true;
        }
        
        return false;
    }

    public void HandleWin()
    {
        if (level.levelSpawn.levelData2.sumBird <= 0)
        {
            winPanel.SetActive(true);
        }
    }


}
[System.Serializable]
public class IdAndNumb
{
    public int id;
    public int numb;
    public List<BirdMechanic> lsBird;
    public List<AnimBird> lsAnimBird;
    public IdAndNumb()
    {
        lsBird = new List<BirdMechanic>();
        lsAnimBird = new List<AnimBird>();
    }
    public void HandleOffBird(Transform param)
    {
        var Temp = Level.Instance.levelLogic;
        //for(int i = 0; i < lsBird.Count; i ++)
        //{
        //    lsAnimBird[i].idElement = lsBird[i].idCowInData;
        //    lsAnimBird[i].gameObject.transform.SetParent(param);
        //}


       
        Debug.LogError("HandleOffBird");
    }
    public void HandleOnBird()
    {

        foreach (var item in lsAnimBird)
        {
            item.gameObject.SetActive(true);
        }


  
    }

}
[System.Serializable]
public class IdAndNumbBirdDuplicate
{
    public int id;
    public int numb;
  
}