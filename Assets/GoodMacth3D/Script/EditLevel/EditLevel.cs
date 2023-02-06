using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditLevel : MonoBehaviour
{
    public List<ElementInLevel> lsElementInLevels;
  
}

[System.Serializable]
 public class ElementInLevel
{
    public int idRow;
    public List<int> lsIdBird;
}
