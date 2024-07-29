using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMonsterSlotInfo : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject kMonsterPick;

    [Header("���� ������")]
    public RectTransform kMonsterNamaTagTrans;
    [Header("���� �̸�")]
    public TMP_Text kMonsterName;

    [Header("���� ���� ����")]
    public RectTransform kMonsterInfoSlot;

    [Header("���� ü��")]
    public TMP_Text kMonsterHp;

    [Header("���� AP")]
    public TMP_Text kMonsterAP;

    [Header("���� �ֻ��� �Ӽ�")]
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

        kMonsterHp.text = $"���� HP : {_mon0.table.Hp}";

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
