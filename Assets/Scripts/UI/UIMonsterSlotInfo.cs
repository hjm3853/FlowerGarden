using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMonsterSlotInfo : MonoBehaviour
{
    [Header("몬스터 선택")]
    public GameObject kMonsterPick;

    [Header("몬스터 이름")]
    public TMP_Text kMonsterName;

    [Header("몬스터 전투 정보")]
    public RectTransform kMonsterInfoSlot;

    [Header("몬스터 체력")]
    public TMP_Text kMonsterHp;

    [Header("몬스터 주사위 기준수")]
    public TMP_Text kMonsterDicePivot;

    [Header("몬스터 주사위 속성")]
    public Transform[] kMonsterRedDiceArr;
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
        kMonsterName.text = $"[{_mon0.table.Name}]";
        Vector3 pos = Camera.main.WorldToScreenPoint(_mon0.GetNameTransform().position);
        kMonsterName.rectTransform.position = pos;

        pos = Camera.main.WorldToScreenPoint(_mon0.GetInfoTransform().position);
        kMonsterInfoSlot.position = pos;

        kMonsterHp.text = $"체력 {_mon0.table.Hp}";

        kMonsterDicePivot.text = _mon0.table.DicePivot.ToString();

        for (int i = 0; i < kMonsterRedDiceArr.Length; i++)
        {
            if (i < _mon0.table.RedDice) kMonsterRedDiceArr[i].gameObject.SetActive(true);
            else kMonsterRedDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr.Length; i++)
        {
            if (i < _mon0.table.RedDice) kMonsterGreenDiceArr[i].gameObject.SetActive(true);
            else kMonsterGreenDiceArr[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr.Length; i++)
        {
            if (i < _mon0.table.RedDice) kMonsterPurpleDiceArr[i].gameObject.SetActive(true);
            else kMonsterPurpleDiceArr[i].gameObject.SetActive(false);
        }
    }

    public void Pick(bool _isPick)
    {
        kMonsterPick.gameObject.SetActive(_isPick);
    }

}
