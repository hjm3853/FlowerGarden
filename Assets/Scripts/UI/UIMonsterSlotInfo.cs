using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMonsterSlotInfo : MonoBehaviour
{
    [Header("몬스터 선택")]
    public GameObject kMonsterPick;

    [Header("몬스터 네임택")]
    public RectTransform kMonsterNamaTagTrans;
    [Header("몬스터 이름")]
    public TMP_Text kMonsterName;

    [Header("몬스터 전투 정보")]
    public RectTransform kMonsterInfoSlot;

    [Header("몬스터 체력")]
    public TMP_Text kMonsterHp;

    [Header("몬스터 AP")]
    public TMP_Text kMonsterAP;

    [Header("몬스터 주사위 속성")]
    public Transform[] kMonsterBlueDiceArr;
    public Transform[] kMonsterGreenDiceArr;
    public Transform[] kMonsterPurpleDiceArr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Monster _mon0)
    {
        kMonsterName.text = $"{_mon0.table.Name}";
        Vector3 pos = Camera.main.WorldToScreenPoint(_mon0.GetNameTransform().position);
        kMonsterNamaTagTrans.position = pos;

        pos = Camera.main.WorldToScreenPoint(_mon0.GetInfoTransform().position);
        kMonsterInfoSlot.position = pos;

        kMonsterHp.text = $"몬스터 HP : {_mon0.table.Hp}";

        kMonsterAP.text = $"AP : {_mon0.table.AP}";

        for (int i = 0; i < kMonsterBlueDiceArr.Length; i++)
        {
            if (i < _mon0.table.BlueToken) kMonsterBlueDiceArr[i].gameObject.SetActive(true);
            else kMonsterBlueDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr.Length; i++)
        {
            if (i < _mon0.table.GreenToken) kMonsterGreenDiceArr[i].gameObject.SetActive(true);
            else kMonsterGreenDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr.Length; i++)
        {
            if (i < _mon0.table.PurpleToken) kMonsterPurpleDiceArr[i].gameObject.SetActive(true);
            else kMonsterPurpleDiceArr[i].gameObject.SetActive(false);
        }
    }

    public void Pick(bool _isPick)
    {
        kMonsterPick.gameObject.SetActive(_isPick);
    }

}
