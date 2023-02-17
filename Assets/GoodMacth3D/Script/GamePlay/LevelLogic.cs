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


        var tempSlot = Instantiate(slotBird, GetPost(indexNewSlot).finalPost.transform.position, Quaternion.identity);

        tempSlot.birdMechanic = bird;
        tempSlot.transform.parent = parentSlotBird;
        bird.transform.parent = tempSlot.transform;
        RotateBird(bird, tempSlot.transform);
        bird.Fly(delegate { SetSlotFinished2(tempSlot); });
  
        listCheckUndo_ItemTileSlots.Add(tempSlot);
        lsSlotBird.Insert(indexNewSlot, tempSlot);

        StartCoroutine(SetListSlot_ResetPosition_Now(0.01f));

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
    
  

    public void SetSlotFinished2(SlotBird slotBird)
    {
        var tempCount = GetIdAndNumb(slotBird.birdMechanic.id);

        tempCount.numb += 1;
        tempCount.lsBird.Add(slotBird);

        foreach (var item in lsIdAndNumb)
        {
            if(item.numb >= 3)
            {
                //if (lsLockDelete.Count > 0)
                //{
                //    return;
                //}
               
                for(int i = 2; i >= 0; i --)
                {
                    lsSlotBird.Remove(item.lsBird[i]);
                }
                for (int i = 2; i >= 0; i--)
                {
                    item.lsBird[i].gameObject.SetActive(false);
                    item.lsBird.RemoveAt(i);
                    item.numb -= 1;
                }
               
                StartCoroutine(SetListSlot_ResetPosition_Now(0.01f));
            }
        }

    }












    public void SetSlotFinished(BirdMechanic bird)
    {
        List<SlotBird> listCheckBird = FindMatch3_Slots(bird);

        if(listCheckBird.Count == 3)
        {
            if(lsLockDelete.Count >0)
            {
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                lsSlotBird.Remove(listCheckBird[i]);
                //Undo
                listCheckUndo_ItemTileSlots.Remove(listCheckBird[i]);
                Destroy(listCheckBird[i].gameObject);
             //   listCheckMatch3Slots[i].SetItemSlot_Match3();
            }
            StartCoroutine(SetListSlot_ResetPosition_Now(0.3f));

        }
       else
        {
            StartCoroutine(CheckGameOver_IEnumerator());
        }


    }



    public List<SlotBird> FindMatch3_Slots(BirdMechanic bird)
    {

        List<SlotBird> lsTempSlot = new List<SlotBird>();

        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            if (lsSlotBird[i].birdMechanic.id == bird.id)
            {
                lsTempSlot.Add(lsSlotBird[i]);

                if (lsTempSlot.Count == 3)
                {
                    return lsTempSlot;
                }
            }
        }

        return lsTempSlot;
    }



    public IEnumerator SetListSlot_ResetPosition_Now(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            lsSlotBird[i].ResetPosSlot(GetPost(i));
        }

    }
    public IEnumerator CheckGameOver_IEnumerator()
    {
        //if (!IsItemTileMoveToSlot()) SoundManager.instance.PlaySound_NoMoreMove();
        yield return new WaitForSeconds(1f);
        if (CheckGameOver())
        {
            SetGameOver();
        }
    }
    public bool IsBirdMoveToSlot()
    {
        if (lsSlotBird.Count >= 7)
        {
            return false;
        }
        return true;
    }
    public bool CheckGameOver()
    {
        if (lsSlotBird.Count < 7)
        {
            return false;
        }

        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            int countItem_ItemTile = CountItemTileSlot_Have_ItemData(lsSlotBird[i]);
            if (countItem_ItemTile == 3) return false;
        }

        return true;
    }

    public int CountItemTileSlot_Have_ItemData(SlotBird itemData)
    {
        int countItemSlot_Have_ItemData = 0;
        for (int i = 0; i < lsSlotBird.Count; i++)
        {
            if (lsSlotBird[i].birdMechanic.id == itemData.birdMechanic.id)
            {
                countItemSlot_Have_ItemData++;
            }
        }

        return countItemSlot_Have_ItemData;
    }

    public void SetGameOver()
    {
        losePanel.SetActive(true);

    }



    private void RotateBird(BirdMechanic paramBird, Transform paramTranform)
    {
        var tempBirdLocalScale = paramBird.animBird.transform.localScale;
        if (paramBird.transform.position.x >= paramTranform.transform.position.x)
        {
            if (paramBird.right)
            {
                paramBird.animBird.transform.localScale = new Vector3(tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
            else
            {
                paramBird.animBird.transform.localScale = new Vector3(-tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
        }
        else
        {
            if (paramBird.right)
            {
                paramBird.animBird.transform.localScale = new Vector3(-tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }
            else
            {
                paramBird.animBird.transform.localScale = new Vector3(tempBirdLocalScale.x, tempBirdLocalScale.y, tempBirdLocalScale.z);
            }

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
                //temp.lsBird.Add(paramBlock);
            //    temp.lsAnimBird.Add(paramBlock.animBird);
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
                    //lsBird.RemoveAt(lsIdAndNumb[i].lsBird[j].idElement);
                    //tesst.RemoveAt(lsIdAndNumb[i].lsBird[j].idElement);
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
              
               // lsIdAndNumb[i].HandleOffBird(this.transform);
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
    public List<SlotBird> lsBird;
   
    public IdAndNumb()
    {
        lsBird = new List<SlotBird>();
     
    }

  
}
[System.Serializable]
public class IdAndNumbBirdDuplicate
{
    public int id;
    public int numb;
  
}