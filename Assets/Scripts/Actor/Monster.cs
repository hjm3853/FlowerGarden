using SheetData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    MonsterTable_Client mTable;
    public MonsterTable_Client table
    {
        get { return mTable; }
        set { mTable = value; }
    }

    public Transform kNameTransform;
    public Transform GetNameTransform() { return kNameTransform; }

    public Transform kInfoTransform;
    public Transform GetInfoTransform() { return kInfoTransform; }

    public int slotIndex { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
