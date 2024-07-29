using EnumDef;
using SheetData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerType kType = PlayerType.None;

    public Transform kDiceTransform;

    public long hp {  get; set; }

    PlayerTable_Client mTable;
    public PlayerTable_Client table
    {
        get { return mTable; }
        set { mTable = value;
            hp = mTable.Hp;
            kType = (PlayerType)mTable.Type;
        }
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
