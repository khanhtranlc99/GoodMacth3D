using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
public class BirdMechanic : MonoBehaviour
{
    public int idCowInData;

   [HideInInspector] public int id;
   [HideInInspector] public int idElement;
    public bool wasLock;
    public BirdMechanic behindBird;
    public GameObject postBird;

    public AnimBird animBird;
    public bool right;

    public void Init()
    {
        
        var SpawnBird = Level.Instance.levelSpawn;
        var CurrentScale = new Vector3();
        id = SpawnBird.levelData2.GetDataLevel(idCowInData);
        animBird = Instantiate(SpawnBird.GetAnimBird(id).animBird, right ? SpawnBird.leftPost.position : SpawnBird.rightPost.position, Quaternion.identity);
        animBird.SetAnim(animBird.FlY, true);
        if(right)
        {
            CurrentScale = new Vector3(-animBird.transform.localScale.x , animBird.transform.localScale.y, animBird.transform.localScale.z);
        }
        else
        {
            CurrentScale = animBird.transform.localScale;
        }

        animBird.transform.SetParent(this.transform);
        animBird.transform.localScale = CurrentScale;
        animBird.transform.DOMove(postBird.transform.position, 0.5f).OnComplete(
            delegate
            {
                animBird.transform.SetParent(postBird.transform);
                animBird.transform.localScale = CurrentScale;
                animBird.SetAnim(animBird.IDLE, true);
            }
         );
 


        if (behindBird != null)
        {
            behindBird.LockClick();
            behindBird.Init();
            behindBird.animBird.SetColor(true);
            behindBird.animBird.SetAnim(behindBird.animBird.IDLE, true);
       
        }
    }




    public void OnClick()
    {
       
        if (wasLock)
        {
          
            return;
        }
        wasLock = true;
        UnlockClickBlockBehide();
        var controler = Level.Instance.levelLogic;
        controler.HandleEndGame(this); 
        controler.SortBird(this);
        controler.lsLockDelete.Add(1);
        animBird.SetAnim(animBird.FlY, true);
        this.transform.DOMove(controler.GetPost(idElement).post.position, 0.2f).OnComplete(delegate
        {
            if (controler.lsLockDelete.Count > 0)
            {
                controler.lsLockDelete.Remove(controler.lsLockDelete[0]);
            }       
            controler.HandleDeleteBirds(this);
            animBird.SetAnim(animBird.IDLE, true);

        });
    
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
        var SpawnBird = Level.Instance.levelSpawn;
        
        if (behindBird != null )
        {

            var CurrentScale = new Vector3();
            var CurrentPossition = new Vector3();
            CurrentPossition = behindBird.transform.position;
            if (SpawnBird.levelData2.GetCountLsDataLevel(idCowInData) > 0)
            {
                behindBird.behindBird = Instantiate(SpawnBird.birdMechanic, CurrentPossition, Quaternion.identity);
                behindBird.behindBird.right = behindBird.right;        
                CurrentScale = behindBird.behindBird.transform.localScale;
                behindBird.behindBird.transform.SetParent(SpawnBird.levelData2.gameObject.transform);
                behindBird.behindBird.transform.localScale = CurrentScale;
                behindBird.behindBird.transform.SetAsFirstSibling();
                behindBird.behindBird.idCowInData = behindBird.idCowInData;
                behindBird.behindBird.Init();
                behindBird.behindBird.animBird.SetColor(true);
               behindBird.behindBird.LockClick();
            }


         
            behindBird.transform.DOJump(this.transform.position, 3, 1, 0.5f).OnComplete(delegate { behindBird.UnlockClick(); });
            behindBird.animBird.SetColor(false);

        }
    }

}
