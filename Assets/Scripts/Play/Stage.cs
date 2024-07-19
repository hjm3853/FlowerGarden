using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    Transform mPurpleSlot;
    Transform mRedSlot;
    Transform mGreenSlot;

    Transform mMonsterSlot0;
    Transform mMonsterSlot1;
    Transform mMonsterSlot2;

    // Start is called before the first frame update
    void Awake()
    {
        mPurpleSlot = transform.Find("PurpleSlot");
        mRedSlot = transform.Find("RedSlot");
        mGreenSlot = transform.Find("GreenSlot");

        mMonsterSlot0 = transform.Find("EnemySlot0");
        mMonsterSlot1 = transform.Find("EnemySlot1");
        mMonsterSlot2 = transform.Find("EnemySlot2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player _purple, Player _red, Player _green)
    {
        _purple.transform.parent = mPurpleSlot;
        _purple.transform.localPosition = Vector3.zero;

        _red.transform.parent = mRedSlot;
        _red.transform.localPosition = Vector3.zero;

        _green.transform.parent = mGreenSlot;
        _green.transform.localPosition = Vector3.zero;
    }

    public void SetMonster(Monster _mon0, Monster _mon1, Monster _mon2)
    {
        _mon0.transform.parent = mMonsterSlot0;
        _mon0.transform.localPosition = Vector3.zero;

        _mon1.transform.parent = mMonsterSlot1;
        _mon1.transform.localPosition = Vector3.zero;

        _mon2.transform.parent = mMonsterSlot2;
        _mon2.transform.localPosition = Vector3.zero;
    }
}
