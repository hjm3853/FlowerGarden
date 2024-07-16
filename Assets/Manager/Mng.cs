using System.Collections;
using System.Collections.Generic;
using EasyExcel;
using UnityEditor.EditorTools;
using UnityEngine;

public class Mng
{
    static public PoolManager pool
    {
        get { return PoolManager.instance; }
    }

    static public PlayManager play
    {
        get { return PlayManager.instance; }
    }

    static public MainCanvas canvas
    {
        get { return MainCanvas.instance; }
    }

    static public TableManager table
    {
        get { return TableManager.instance; }
    }

    static public DataManager data
    {
        get { return DataManager.instance; }
    }
}
