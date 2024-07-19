using SheetData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform kDiceTransform;

    PlayerTable_Client mTable;
    public PlayerTable_Client table
    {
        get { return mTable; }
        set { mTable = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetDiceTransform() { return kDiceTransform; }
}
