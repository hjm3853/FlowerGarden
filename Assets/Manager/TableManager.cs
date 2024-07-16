using System.Collections.Generic;
using EasyExcel;
using SheetData;
using UnityEngine;


public class TableManager : MonoBehaviour
{
    static public TableManager instance = null;

    private EEDataManager _ee = new EEDataManager();

    //////////////////////////////////////////////////////////////////////////////////
    ///������

    private List<GameDataTable_Client> mGameDataTableList;
    public List<GameDataTable_Client> gameDataTableList { get { return mGameDataTableList; } }

    private void Awake()
    {
        instance = this;

        //////////////////////////////////////////////////////////////////////////////////
        //�÷��̾� ���� ���� ���̺�
        //layerTableList = _ee.GetListJson<PlayerTable_Client>();
        mGameDataTableList = _ee.GetListJson<GameDataTable_Client>();
    }
/*
    public void Init()
    {
        //////////////////////////////////////////////////////////////////////////////////
        //�÷��̾� ���� ���� ���̺�
        //layerTableList = _ee.GetListJson<PlayerTable_Client>();
        mGameDataTableList = _ee.GetListJson<GameDataTable_Client>();

    }*/
/*
    /// <summary>ĳ�������� UID�� ��������</summary>
    public PlayerTable_Client FindPlayerInfo(long _uid)
    {
        PlayerTable_Client data = mPlayerTableList.Find(d => d.UID == _uid);
        if (data != default) return data;
#if LOG
		Log.Error($"UID [{_uid}] �� �´� �����Ͱ� �����ϴ�", Log.Account.Default, 1);
#endif
        return default;
    }
*/
}