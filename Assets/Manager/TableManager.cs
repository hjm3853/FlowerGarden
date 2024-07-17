using System.Collections.Generic;
using EasyExcel;
using SheetData;
using UnityEngine;


public class TableManager : MonoBehaviour
{
    static TableManager mInstance = null;

    private EEDataManager _ee = new EEDataManager();

    //////////////////////////////////////////////////////////////////////////////////
    ///원본형

    private List<StageTable_Client> mStageTableList;
    public List<StageTable_Client> stageTableList { get { return mStageTableList; } }

    private List<StageGroupTable_Client> mStageGroupTableList;
    public List<StageGroupTable_Client> stageGroupTableList { get { return mStageGroupTableList; } }

    static public TableManager instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<TableManager>(true);
                mInstance.Init();
            }

            return mInstance;
        }        
    }

    private void Awake()
    {
        //Init();
    }

    public void Init()
    {
        //////////////////////////////////////////////////////////////////////////////////
        //플레이어 액터 관련 테이블
        //layerTableList = _ee.GetListJson<PlayerTable_Client>();
        mStageTableList = _ee.GetListJson<StageTable_Client>();
        mStageGroupTableList = _ee.GetListJson<StageGroupTable_Client>();
    }
    
    public StageTable_Client FindStageTable(long _uid)
    {
        StageTable_Client data = mStageTableList.Find(d => d.UID == _uid);
        if (data != default) return data;
#if LOG
		Log.Error($"UID [{_uid}] 와 맞는 데이터가 없습니다", Log.Account.Default, 1);
#endif
        return default;
    }

    public StageGroupTable_Client FindStageGroupTable(long _uid)
    {
        StageGroupTable_Client data = mStageGroupTableList.Find(d => d.UID == _uid);
        if (data != default) return data;
#if LOG
		Log.Error($"UID [{_uid}] 와 맞는 데이터가 없습니다", Log.Account.Default, 1);
#endif
        return default;
    }
}