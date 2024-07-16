using System.Collections.Generic;
using EasyExcel;
using SheetData;
using UnityEngine;


public class TableManager : MonoBehaviour
{
    static public TableManager instance = null;

    private EEDataManager _ee = new EEDataManager();

    //////////////////////////////////////////////////////////////////////////////////
    ///원본형

    private List<GameDataTable_Client> mGameDataTableList;
    public List<GameDataTable_Client> gameDataTableList { get { return mGameDataTableList; } }

    private void Awake()
    {
        instance = this;

        //////////////////////////////////////////////////////////////////////////////////
        //플레이어 액터 관련 테이블
        //layerTableList = _ee.GetListJson<PlayerTable_Client>();
        mGameDataTableList = _ee.GetListJson<GameDataTable_Client>();
    }
/*
    public void Init()
    {
        //////////////////////////////////////////////////////////////////////////////////
        //플레이어 액터 관련 테이블
        //layerTableList = _ee.GetListJson<PlayerTable_Client>();
        mGameDataTableList = _ee.GetListJson<GameDataTable_Client>();

    }*/
/*
    /// <summary>캐릭터정보 UID로 가져오기</summary>
    public PlayerTable_Client FindPlayerInfo(long _uid)
    {
        PlayerTable_Client data = mPlayerTableList.Find(d => d.UID == _uid);
        if (data != default) return data;
#if LOG
		Log.Error($"UID [{_uid}] 와 맞는 데이터가 없습니다", Log.Account.Default, 1);
#endif
        return default;
    }
*/
}