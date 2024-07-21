using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    static public PlayManager instance = null;

    public Stage kStage;

    public long TestPlayerUID0 = 300000000001;
    public long TestPlayerUID1 = 300000000002;
    public long TestPlayerUID2 = 300000000003;

    public long TestMonsterUID0 = 400000000001;
    public long TestMonsterUID1 = 400000000002;
    public long TestMonsterUID2 = 400000000003;

    public Player[] playerList = new Player[3];
    public Monster[] monsterList = new Monster[3];

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerSetting(TestPlayerUID0, TestPlayerUID1, TestPlayerUID2);
        MonsterSetting(TestMonsterUID0, TestMonsterUID1, TestMonsterUID2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerSetting(long _purpleUID, long _redUID, long _greenUID)
    {
        Player player0 = null;
        Player player1 = null;
        Player player2 = null;

        if (_purpleUID != 0)
        {
            var table = Mng.table.FindPlayerTableList(_purpleUID);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            player0 = Instantiate(go).GetComponent<Player>();
            player0.table = table;
        }

        if (_redUID != 0)
        {
            var table = Mng.table.FindPlayerTableList(_redUID);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            player1 = Instantiate(go).GetComponent<Player>();
            player1.table = table;
        }

        if (_greenUID != 0)
        {
            var table = Mng.table.FindPlayerTableList(_greenUID);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            player2 = Instantiate(go).GetComponent<Player>();
            player2.table = table;
        }

        kStage.SetPlayer(player0, player1, player2);
        Mng.canvas.stageInfo.SetPlayer(player0, player1, player2);
    }

    public void MonsterSetting(long _uid0, long _uid1, long _uid2)
    {
        for(int i = 0; i < monsterList.Length; i++)
            monsterList[i] = null;
        

        if (_uid0 != 0)
        {
            var table = Mng.table.FindMonsterTableList(_uid0);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            monsterList[0] = Instantiate(go).GetComponent<Monster>();
            monsterList[0].table = table;
        }

        if (_uid1 != 0)
        {
            var table = Mng.table.FindMonsterTableList(_uid1);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            monsterList[1] = Instantiate(go).GetComponent<Monster>();
            monsterList[1].table = table;
        }

        if (_uid2 != 0)
        {
            var table = Mng.table.FindMonsterTableList(_uid2);
            GameObject go = Resources.Load(table.Prefab) as GameObject;
            monsterList[2] = Instantiate(go).GetComponent<Monster>();
            monsterList[2].table = table;
        }

        kStage.SetMonster(monsterList);
        Mng.canvas.stageInfo.SetMonster(monsterList[0], monsterList[1], monsterList[2]);
    }    
}