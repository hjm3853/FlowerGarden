using SheetData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageSelect : MonoBehaviour
{
    public Transform kStageGridGroupTrans;
    Button [] mStageButtonArr;
    
    StageGroupTable_Client mSelectTable;

    public GameObject kStagePopupGo;
    public TMP_Text kStageNameTxt;
    public TMP_Text kStageDescTxt;

    // Start is called before the first frame update
    void Awake()
    {        
        mStageButtonArr = kStageGridGroupTrans.GetComponentsInChildren<Button>(true);
        foreach (var btn in mStageButtonArr)
            btn.gameObject.SetActive(false);

        kStageGridGroupTrans.gameObject.SetActive(true);

        int cnt = Mng.table.stageGroupTableList.Count;
        int selectIndex = Random.Range(0, cnt);
        mSelectTable = Mng.table.stageGroupTableList[selectIndex];

        var main = mStageButtonArr[mSelectTable.Main_SlotIndex];
        main.gameObject.SetActive(true);
        main.GetComponentInChildren<TMP_Text>().text = "����";

        var battle1 = mStageButtonArr[mSelectTable.Battle1_SlotIndex];
        battle1.gameObject.SetActive(true);
        battle1.GetComponentInChildren<TMP_Text>().text = "����1";

        var battle2 = mStageButtonArr[mSelectTable.Battle2_SlotIndex];
        battle2.gameObject.SetActive(true);
        battle2.GetComponentInChildren<TMP_Text>().text = "����2";

        var event1 = mStageButtonArr[mSelectTable.Event1_SlotIndex];
        event1.gameObject.SetActive(true);
        event1.GetComponentInChildren<TMP_Text>().text = "�̺�Ʈ1";

        var event2 = mStageButtonArr[mSelectTable.Event2_SlotIndex];
        event2.gameObject.SetActive(true);
        event2.GetComponentInChildren<TMP_Text>().text = "�̺�Ʈ2";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectButtonClick(Button _btn)
    {
        kStagePopupGo.SetActive(true);
        StageTable_Client stageTable = null;

        int selectIndex = _btn.transform.GetSiblingIndex();

        if (mSelectTable.Main_SlotIndex == selectIndex)
        {
            Debug.Log("���� �������� ����");
            stageTable = Mng.table.FindStageTable(mSelectTable.Main_UID);            
        }
        else if ( mSelectTable.Battle1_SlotIndex  == selectIndex )
        {
            Debug.Log("��Ʋ1 �������� ����");
            stageTable = Mng.table.FindStageTable(mSelectTable.Battle1_UID);
        }
        else if (mSelectTable.Battle2_SlotIndex == selectIndex)
        {
            Debug.Log("��Ʋ2 �������� ����");
            stageTable = Mng.table.FindStageTable(mSelectTable.Battle2_UID);
        }
        else if (mSelectTable.Event1_SlotIndex == selectIndex)
        {
            Debug.Log("�̺�Ʈ1 �������� ����");
            stageTable = Mng.table.FindStageTable(mSelectTable.Event1_UID);
        }
        else if (mSelectTable.Event2_SlotIndex == selectIndex)
        {
            Debug.Log("�̺�Ʈ2 �������� ����");
            stageTable = Mng.table.FindStageTable(mSelectTable.Event2_UID);
        }

        kStageNameTxt.text = stageTable.Name;
        kStageDescTxt.text = stageTable.Description;
    }

    public void OnStageEnterClick()
    {
        kStagePopupGo.SetActive(false);
    }
}
