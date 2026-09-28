using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PieceStyle
{
    public PieceType type;
    public Sprite Sprite;
}

public enum PieceType
{
    Blue,
    Red,
    Green,
    Yellow,
    Grey,
    Purple

}
[CreateAssetMenu(fileName = "PieceData")]
public class PieceData : ScriptableObject
{
    public string styleName;
    public List<PieceStyle> data;
}

