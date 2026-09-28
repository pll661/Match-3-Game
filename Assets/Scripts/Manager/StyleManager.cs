using System.Collections.Generic;
using UnityEngine;

public class StyleManager : MonoBehaviour
{
    public static StyleManager Instance;
    [SerializeField] private PieceData currentStyle;
    [SerializeField] private List<PieceData> allStyles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
    private void Start()
    {
        LoadStyle();
    }
    public void LoadStyle()
    {
        string styleName = PlayerPrefs.GetString("STYLENAME");
        if (string.IsNullOrEmpty(styleName))
        {
            //Debug.LogError("找不到皮肤数据");
            currentStyle = allStyles[0];
            return;
        }
        currentStyle = GetStyleDataByName(styleName);
    }
    public void ChangedStyle(PieceData newStyle)
    {
        if (newStyle == null) return;
        currentStyle = newStyle;
        SaveStyleData();
    }
    public PieceStyle GetTypeData(PieceType t)
    {
        if (currentStyle == null)
        {
            Debug.LogError("StyleManager：当前没有设置皮肤！");
            return null;
        }
        foreach (PieceStyle data in currentStyle.data)
        {
            if (data.type == t)
                return data;
        }
        return null;
    }
    public PieceData GetStyleDataByName(string name)
    {
        if (allStyles != null && allStyles.Count > 0)
        {
            foreach (PieceData data in allStyles)
            {
                if (data.styleName == name)
                {
                    return data;
                }
            }
        }
        return null;
    }
    public void SaveStyleData()
    {
        if (currentStyle == null) return;
        PlayerPrefs.SetString("STYLENAME", currentStyle.styleName);
        PlayerPrefs.Save();
    }
    public PieceData GetcurrentStyle()
    {
        if (currentStyle == null) return null;
        return currentStyle;

    }
}
