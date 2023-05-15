using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/SOExample1")]
public class SOExample1 : ScriptableObject
{
    public int value1;
    public float value2;
    private bool value3;
    public bool value4;
    [SerializeField]
    private string value5;
    private ScriptableObject value6;
    [SerializeField]
    private ExampleStruct1 _value7;
    [SerializeField]
    private ScriptableObject value8;
    [SerializeField]
    private List<bool> _value9;
}

[System.Serializable]
public struct ExampleStruct1
{
    public int value1;
    public string value2;
}
