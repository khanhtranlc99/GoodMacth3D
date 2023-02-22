using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using System;
public class BirdMechanic : MonoBehaviour
{
    [HideInInspector] public int idCowInData;

    public int id;
   [HideInInspector] public int idElement;
    public bool wasLock;
    public BirdMechanic behindBird;
    public GameObject postBird;
    public AnimBird animBird;
    public BoxCollider2D boxCollider2;
    public SpriteRenderer note;
    public bool right;
    public int orderIndex;
    public SpriteRenderer dot;
    public Vector3 postWhenBirdMove;
    public Post post;
    public SlotBird slotBird;

    public void Init()
    {
          dot.color = new Color32(0, 0, 0, 0);
        postWhenBirdMove = new Vector3();
        var SpawnBird = Level.Instance.levelSpawn;
        var CurrentScale = new Vector3();
        id = SpawnBird.levelData2.GetDataLevel(idCowInData); //lay ra va remove id khoi data
        animBird = SimplePool2.Spawn(SpawnBird.GetAnimBird(id).animBird, right ? SpawnBird.leftPost.position : SpawnBird.rightPost.position, Quaternion.identity);
        animBird.SetAnim(animBird.FlY, true);
        if (right)
        {
            CurrentScale = new Vector3(-animBird.transform.localScale.x, animBird.transform.localScale.y, animBird.transform.localScale.z);
        }
        else
        {
            CurrentScale = animBird.transform.localScale;
        }

        animBird.transform.SetParent(this.transform);
        animBird.transform.localScale = CurrentScale;
        Ease easy = Ease.OutSine;
        int random = UnityEngine.Random.Range(1, 6);
        if (random == 1)
            easy = Ease.OutQuad;
        else if (random == 2)
            easy = Ease.OutQuart;
        else if (random == 3)
            easy = Ease.OutFlash;
        else if (random == 4)
            easy = Ease.InOutFlash;

        animBird.transform.DOMove(new Vector3(postBird.transform.position.x, postBird.transform.position.y+0.15f, postBird.transform.position.z), 1.5f).SetEase(easy).OnComplete(delegate {
            animBird.transform.DOMove(postBird.transform.position, 0.2f).OnComplete(
              delegate
              {
                  animBird.transform.SetParent(postBird.transform);
                  animBird.transform.localScale = CurrentScale;
                  animBird.SetAnim(animBird.IDLE, true);
                  animBird.transform.position = postBird.gameObject.transform.position;
                 
              }
           );
        }); 
        animBird.SetOrderInLayer(2);
        orderIndex = 2;

        if (behindBird != null)
        {
            behindBird.LockClick();
            behindBird.Init();
            behindBird.animBird.SetColor(true);
            behindBird.animBird.SetOrderInLayer(1);
            behindBird.orderIndex = 1;


        }

    }

   
    public void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        if(!wasLock)
        {
            Debug.Log("wasLock");
            OnClick();
        }

    }


    public void OnClick()
    {
        wasLock = true;
        UnlockClickBlockBehide();
        var controler = Level.Instance.levelLogic;
        controler.HandleCheckLose(this);
        controler.AddBirdToListSlot(this);
        animBird.transform.position = postBird.gameObject.transform.position;
        //animBird.SetAnim(animBird.FlY, true);

    
    }


    public void Fly(Action action)
    {
        var controler = Level.Instance.levelLogic;
        DOTween.Kill(this.transform);
        animBird.SetAnim(animBird.FlY, true);
        Level.Instance.levelLogic.lsLoockBooster.Add(1);
        this.transform.DOLocalMove(new Vector3(0,0.2f,0), 1).OnComplete(delegate
        {
            animBird.SetAnim(animBird.IDLE, true);
            this.transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f).OnComplete(
            delegate {
                if (Level.Instance.levelLogic.lsLoockBooster.Count > 0)
                {
                    Level.Instance.levelLogic.lsLoockBooster.Remove(Level.Instance.levelLogic.lsLoockBooster[0]);
                }
                if (action != null)
                {
                    action?.Invoke();
                }
            });          
        }).SetEase(Ease.InOutFlash);
    }
   


    public void UnlockClick()
    {
        wasLock = false;
        boxCollider2.enabled = true;
    }
    public void LockClick()
    {
        wasLock = true;
        boxCollider2.enabled = false;
    }

    public void UnlockClickBlockBehide()
    {
        var SpawnBird = Level.Instance.levelSpawn;
        
        if (behindBird != null)
        {

            var CurrentScale = new Vector3();
            var CurrentPossition = new Vector3();
            CurrentPossition = behindBird.transform.position;
            if (SpawnBird.levelData2.GetCountLsDataLevel(idCowInData) > 0)
            {
                behindBird.behindBird = SimplePool2.Spawn(SpawnBird.birdMechanic, CurrentPossition, Quaternion.identity);
                behindBird.behindBird.right = behindBird.right;        
                CurrentScale = behindBird.behindBird.transform.localScale;
                behindBird.behindBird.transform.SetParent(SpawnBird.levelData2.gameObject.transform);
                behindBird.behindBird.transform.localScale = CurrentScale;
                behindBird.behindBird.idCowInData = behindBird.idCowInData;
                behindBird.behindBird.Init();
                behindBird.behindBird.animBird.SetColor(true);
                behindBird.behindBird.orderIndex = behindBird.orderIndex;
                behindBird.behindBird.animBird.SetOrderInLayer(behindBird.orderIndex);
                behindBird.behindBird.LockClick();
                SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInTheBack = behindBird.behindBird;

            }
            else
            {
                SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInTheBack = null;
            }
            SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInFront = behindBird;
          

            behindBird.orderIndex = this.orderIndex;
            behindBird.animBird.SetOrderInLayer(this.orderIndex);
            behindBird.animBird.SetAnim(animBird.IDLE, true); 
            behindBird.animBird.SetColor(false);
            DOTween.Kill(behindBird.transform);
            behindBird.transform.DOJump(this.transform.position, 0.5f, 1, 0.5f).OnComplete(delegate {
                behindBird.UnlockClick();
                behindBird.animBird.SetAnim(animBird.IDLE, true);
                behindBird = null;
            });
         


        }
        else
        {
            SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInFront = null;
        }
    }
    public void HandleBirdBehindByBooster()
    {
        var SpawnBird = Level.Instance.levelSpawn;
        var DoubleBird = SpawnBird.levelData2.GetDoubleBird(idCowInData);
        var CurrentScale = new Vector3();
        var CurrentPossition = new Vector3();
        CurrentPossition = this.transform.position;
        if (SpawnBird.levelData2.GetCountLsDataLevel(idCowInData) > 0)
        {
     
          var tempBird = SimplePool2.Spawn(SpawnBird.birdMechanic, CurrentPossition, Quaternion.identity);
            tempBird.right = this.right;
            CurrentScale = tempBird.transform.localScale;
            tempBird.transform.SetParent(SpawnBird.levelData2.gameObject.transform);
            tempBird.transform.localScale = CurrentScale;
            tempBird.idCowInData = this.idCowInData;
            tempBird.Init();
            tempBird.animBird.SetColor(true);
            tempBird.orderIndex = this.orderIndex;
            tempBird.animBird.SetOrderInLayer(this.orderIndex);
            tempBird.LockClick();
            var tempBirdFront = DoubleBird.birdInFront;
            OnClick();
            DoubleBird.birdInFront = tempBirdFront;   
            DoubleBird.birdInFront.behindBird = tempBird;
            DoubleBird.birdInTheBack = tempBird;
            Debug.LogError("tempBird");
        }
        else
        {
            OnClickBirdBehindByBooser();
            Debug.LogError("OnClickBirdBehindByBooser");
        }
        
    }    
    private void OnClickBirdBehindByBooser()
    {
        wasLock = true;
        UnlockClickBlockBehideBooster();
        var controler = Level.Instance.levelLogic;
        controler.HandleCheckLose(this);
        controler.AddBirdToListSlot(this);
        animBird.transform.position = postBird.gameObject.transform.position;
    }
    public void UnlockClickBlockBehideBooster()
    {
        var SpawnBird = Level.Instance.levelSpawn;

        SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInTheBack = null;
        SpawnBird.levelData2.GetDoubleBird(idCowInData).birdInFront.behindBird = null;
    }
}
