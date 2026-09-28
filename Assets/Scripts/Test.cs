using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int a = 5;
        ChangeInt(a);                              // Ô¤²â a = 5
        Debug.Log(a);
        var l1 = new List<int> { 1 };
        AddOne(l1);                                // Ô¤²â l1 = 2
        Debug.Log(l1.Count);
        var l2 = new List<int> { 1 };
        Replace(l2);
        Debug.Log(l2.Count);                       //1
    }

    // Update is called once per frame
    void Update()
    {


    }
    void ChangeInt(int x) { x = 99; }
    void AddOne(List<int> list) { list.Add(1); }
    void Replace(List<int> list) { list = new List<int> { 9 }; }

   
}
