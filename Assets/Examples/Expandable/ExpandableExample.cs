using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableExample : MonoBehaviour
{
    [Expandable]
    public int value1;
    [Expandable]
    public bool value2;
    [Expandable]
    public ExampleClass value3;
    [Expandable]
    public SOExample1 value4;
    [Expandable]
    public List<int> value5;
    //[Expandable]
    //public 
}

[System.Serializable]
public class ExampleClass
{
    public string id;
    public int value;
    public bool condition;
}
