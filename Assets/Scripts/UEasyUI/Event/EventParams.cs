using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于事件传递多个参数
/// </summary>
public class EventParams
{
    public bool BoolValue1;
    public bool BoolValue2;
    public bool BoolValue3;
    public bool BoolValue4;
    public bool BoolValue5;

    public int IntValue1;
    public int IntValue2;
    public int IntValue3;
    public int IntValue4;
    public int IntValue5;

    public long LongValue1;
    public long LongValue2;
    public long LongValue3;
    public long LongValue4;
    public long LongValue5;

    public string StrValue1;
    public string StrValue2;
    public string StrValue3;
    public string StrValue4;
    public string StrValue5;

    public void Reset()
    {
        IntValue1 = IntValue2 = IntValue3 = IntValue4 = IntValue5 = 0;
        LongValue1 = LongValue2 = LongValue3 = LongValue4 = LongValue5 = 0;
        StrValue1 = StrValue2 = StrValue3 = StrValue4 = StrValue5 = null;
    }
}