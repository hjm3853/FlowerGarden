using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    [Header("몬스터 Hp(sprite)-75% 이상")]
    public Image kHpSpr75;
    [Header("몬스터 Hp(sprite)-50% 이상")]
    public Image kHpSpr50;
    [Header("몬스터 Hp(sprite)-25% 이상")]
    public Image kHpSpr25;
    [Header("몬스터 Hp(sprite)-0% 이상")]
    public Image kHpSpr00;

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

    public void Set(Monster _mon)
    {
        kMonsterName.text = $"{_mon.table.Name}";
        Vector3 pos = Camera.main.WorldToScreenPoint(_mon.GetNameTransform().position);
        kMonsterNamaTagTrans.position = pos;

        pos = Camera.main.WorldToScreenPoint(_mon.GetInfoTransform().position);
        kMonsterInfoSlot.position = pos;

        kMonsterHp.text = $"HP : {_mon.hp}";
        kHpSpr75.enabled = false;
        kHpSpr50.enabled = false;
        kHpSpr25.enabled = false;
        kHpSpr00.enabled = false;
        float ratio = (float)_mon.hp / (float)_mon.table.Hp;

        if (ratio >= 0.75f)
        {
            kHpSpr75.enabled = true;
        }
        else if (ratio >= 0.5f)
        {
            kHpSpr50.enabled = true;
        }
        else if (ratio >= 0.25f)
        {
            kHpSpr25.enabled = true;
        }
        else
        {
            kHpSpr00.enabled = true;
        }

        kMonsterAP.text = $"AP : {_mon.table.AP}";

        for (int i = 0; i < kMonsterBlueDiceArr.Length; i++)
        {
            if (i < _mon.table.BlueToken) kMonsterBlueDiceArr[i].gameObject.SetActive(true);
            else kMonsterBlueDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr.Length; i++)
        {
            if (i < _mon.table.GreenToken) kMonsterGreenDiceArr[i].gameObject.SetActive(true);
            else kMonsterGreenDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr.Length; i++)
        {
            if (i < _mon.table.PurpleToken) kMonsterPurpleDiceArr[i].gameObject.SetActive(true);
            else kMonsterPurpleDiceArr[i].gameObject.SetActive(false);
        }
    }

    public void Pick(bool _isPick)
    {
        kMonsterPick.gameObject.SetActive(_isPick);
    }

}
