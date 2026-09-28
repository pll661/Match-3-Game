using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseStyleUI : MonoBehaviour
{
  




    public void ChangeStyle(PieceData data)
    {
        StyleManager.Instance.ChangedStyle(data);
    }
    public void BackHome()
    {
        UIManager.Instance.ShowMainPanel();
    }
}
