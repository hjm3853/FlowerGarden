using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStageInfo : MonoBehaviour
{
    public RectTransform kPurpleDiceSlot;
    public TMP_Text kPurpleDiceCnt;

    public RectTransform kRedDiceSlot;
    public TMP_Text kRedDiceCnt;

    public RectTransform kGreenDiceSlot;
    public TMP_Text kGreenDiceCnt;

    public TMP_Text kPlayerTotalHp;

    
    public RectTransform kMonsterInfoSlot0;
    public RectTransform kMonsterInfoSlot1;
    public RectTransform kMonsterInfoSlot2;

    public TMP_Text kMonsterName0;
    public TMP_Text kMonsterName1;
    public TMP_Text kMonsterName2;

    public TMP_Text kMonsterHp0;
    public TMP_Text kMonsterHp1;
    public TMP_Text kMonsterHp2;

    public TMP_Text kMonsterDicePivot0;
    public TMP_Text kMonsterDicePivot1;
    public TMP_Text kMonsterDicePivot2;

    public Transform[] kMonsterRedDiceArr0;
    public Transform[] kMonsterGreenDiceArr0;
    public Transform[] kMonsterPurpleDiceArr0;

    public Transform[] kMonsterRedDiceArr1;
    public Transform[] kMonsterGreenDiceArr1;
    public Transform[] kMonsterPurpleDiceArr1;

    public Transform[] kMonsterRedDiceArr2;
    public Transform[] kMonsterGreenDiceArr2;
    public Transform[] kMonsterPurpleDiceArr2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player _purplePlayer, Player _redPlayer, Player _greenPlayer)
    {
        kPurpleDiceCnt.text = _purplePlayer.table.Dice.ToString();
        kRedDiceCnt.text = _redPlayer.table.Dice.ToString();
        kGreenDiceCnt.text = _greenPlayer.table.Dice.ToString();

        kPlayerTotalHp.text = (_purplePlayer.table.Hp + _redPlayer.table.Hp + _greenPlayer.table.Hp).ToString();

        Vector3 pos = Camera.main.WorldToScreenPoint(_purplePlayer.GetDiceTransform().position);
        kPurpleDiceSlot.position = pos;

        pos = Camera.main.WorldToScreenPoint(_redPlayer.GetDiceTransform().position);
        kRedDiceSlot.position = pos;

        pos = Camera.main.WorldToScreenPoint(_greenPlayer.GetDiceTransform().position);
        kGreenDiceSlot.position = pos;
    }

    public void SetMonster(Monster _mon0, Monster _mon1, Monster _mon2)
    {
        kMonsterName0.text = $"[{_mon0.table.Name}]";
        Vector3 pos = Camera.main.WorldToScreenPoint(_mon0.GetNameTransform().position);
        kMonsterName0.rectTransform.position = pos;

        kMonsterName1.text = $"[{_mon1.table.Name}]";
        pos = Camera.main.WorldToScreenPoint(_mon1.GetNameTransform().position);
        kMonsterName1.rectTransform.position = pos;

        kMonsterName2.text = $"[{_mon2.table.Name}]";
        pos = Camera.main.WorldToScreenPoint(_mon2.GetNameTransform().position);
        kMonsterName2.rectTransform.position = pos;

        pos = Camera.main.WorldToScreenPoint(_mon0.GetInfoTransform().position);
        kMonsterInfoSlot0.position = pos;
        pos = Camera.main.WorldToScreenPoint(_mon1.GetInfoTransform().position);
        kMonsterInfoSlot1.position = pos;
        pos = Camera.main.WorldToScreenPoint(_mon2.GetInfoTransform().position);
        kMonsterInfoSlot2.position = pos;

        kMonsterHp0.text = $"체력 {_mon0.table.Hp}";
        kMonsterHp1.text = $"체력 {_mon1.table.Hp}";
        kMonsterHp2.text = $"체력 {_mon2.table.Hp}";

        kMonsterDicePivot0.text = _mon0.table.DicePivot.ToString();
        kMonsterDicePivot1.text = _mon1.table.DicePivot.ToString();
        kMonsterDicePivot2.text = _mon2.table.DicePivot.ToString();

        for(int i = 0; i < kMonsterRedDiceArr0.Length; i++){
            if( i < _mon0.table.RedDice )   kMonsterRedDiceArr0[i].gameObject.SetActive(true);
            else                            kMonsterRedDiceArr0[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr0.Length; i++){
            if (i < _mon0.table.RedDice)    kMonsterGreenDiceArr0[i].gameObject.SetActive(true);
            else                            kMonsterGreenDiceArr0[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr0.Length; i++){
            if (i < _mon0.table.RedDice)    kMonsterPurpleDiceArr0[i].gameObject.SetActive(true);
            else                            kMonsterPurpleDiceArr0[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < kMonsterRedDiceArr1.Length; i++){
            if (i < _mon1.table.RedDice)    kMonsterGreenDiceArr1[i].gameObject.SetActive(true);
            else                            kMonsterGreenDiceArr1[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr1.Length; i++){
            if (i < _mon1.table.RedDice)    kMonsterGreenDiceArr1[i].gameObject.SetActive(true);
            else                            kMonsterGreenDiceArr1[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr1.Length; i++){
            if (i < _mon1.table.RedDice)    kMonsterPurpleDiceArr1[i].gameObject.SetActive(true);
            else                            kMonsterPurpleDiceArr1[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < kMonsterRedDiceArr2.Length; i++){
            if (i < _mon2.table.RedDice)    kMonsterGreenDiceArr2[i].gameObject.SetActive(true);
            else                            kMonsterGreenDiceArr2[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterGreenDiceArr2.Length; i++){
            if (i < _mon2.table.RedDice)    kMonsterGreenDiceArr2[i].gameObject.SetActive(true);
            else                            kMonsterGreenDiceArr2[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < kMonsterPurpleDiceArr2.Length; i++){
            if (i < _mon2.table.RedDice)    kMonsterPurpleDiceArr2[i].gameObject.SetActive(true);
            else                            kMonsterPurpleDiceArr2[i].gameObject.SetActive(false);
        }
    }
}
