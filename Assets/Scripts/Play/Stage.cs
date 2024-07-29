using EnumDef;
using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

public class Stage : MonoBehaviour
{
    Transform mPurpleSlot;
    Transform mRedSlot;
    Transform mGreenSlot;

    Transform mMonsterSlot0;
    Transform mMonsterSlot1;
    Transform mMonsterSlot2;

    Transform mBattleTrans;
    Transform mPlayerBattleTrans;
    Transform mMonsterBattleTrans;

    public PlayerDiceBoard kPlayerDiceBoard;

    // Start is called before the first frame update
    void Awake()
    {
        mPurpleSlot = transform.Find("Set/PurpleSlot");
        mRedSlot = transform.Find("Set/RedSlot");
        mGreenSlot = transform.Find("Set/GreenSlot");

        mMonsterSlot0 = transform.Find("Set/EnemySlot0");
        mMonsterSlot1 = transform.Find("Set/EnemySlot1");
        mMonsterSlot2 = transform.Find("Set/EnemySlot2");

        mPlayerBattleTrans = transform.Find("Battle/Player");
        mMonsterBattleTrans = transform.Find("Battle/Monster");

        mBattleTrans = transform.Find("Battle");

        EasyTouch.On_TouchUp += OnTouchUp;
    }

    private void OnTouchUp(Gesture gesture)
    {
        if(gesture.pickedObject == null)
            return;

        if( gesture.pickedObject.layer == LayerMask.NameToLayer("Player") )
        {
            var player = gesture.pickedObject.GetComponent<Player>();
            Mng.canvas.stageInfo.SetPlayerPick(player);

            Debug.Log("Player Touch");
        }
        else if(gesture.pickedObject.layer == LayerMask.NameToLayer("Monster"))
        {
            var mon = gesture.pickedObject.GetComponent<Monster>();
            Mng.canvas.stageInfo.SetMonsterPick(mon);

            Debug.Log("Monster Touch");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(params Player [] _players)
    {
        foreach( Player player in _players )
        {
            switch (player.kType)
            {
                case PlayerType.Blue:
                    player.transform.parent = mRedSlot;
                    player.transform.localPosition = Vector3.zero;
                    break;
                case PlayerType.Purple:
                    player.transform.parent = mPurpleSlot;
                    player.transform.localPosition = Vector3.zero;
                    break;
                case PlayerType.Green:
                    player.transform.parent = mGreenSlot;
                    player.transform.localPosition = Vector3.zero;
                    break;
            }
        }
    }

    public void SetMonster(Monster [] _monsters)
    {
        for(int i = 0; i < _monsters.Length; i++)
        {
            Monster mon = _monsters[i];
            if (mon == null)
                continue;

            mon.slotIndex = i;

            switch(i){
                case 0: mon.transform.parent = mMonsterSlot0; break;
                case 1: mon.transform.parent = mMonsterSlot1; break;
                case 2: mon.transform.parent = mMonsterSlot2; break;
            }

            mon.transform.localPosition = Vector3.zero;
        }
    }

    public void SetBase()
    {
        mPurpleSlot.gameObject.SetActive(true);
        mRedSlot.gameObject.SetActive(true);
        mGreenSlot.gameObject.SetActive(true);

        mMonsterSlot0.gameObject.SetActive(true);
        mMonsterSlot1.gameObject.SetActive(true);
        mMonsterSlot2.gameObject.SetActive(true);

        mBattleTrans.gameObject.SetActive(false);

        foreach (var player in Mng.play.playerList)
            player.transform.localPosition = Vector3.zero;

        foreach (var mon in Mng.play.monsterList)
            mon.transform.localPosition = Vector3.zero;
    }

    public void SetBattle(Player _player, Monster _monster)
    {
        mPurpleSlot.gameObject.SetActive(false);        
        mRedSlot.gameObject.SetActive(false);
        mGreenSlot.gameObject.SetActive(false);

        mMonsterSlot0.gameObject.SetActive(false);
        mMonsterSlot1.gameObject.SetActive(false);
        mMonsterSlot2.gameObject.SetActive(false);

        mBattleTrans.gameObject.SetActive(true);

        switch(_player.kType)
        {
            case PlayerType.Purple:
                mPurpleSlot.gameObject.SetActive(true);
                break;
            case PlayerType.Blue:
                mRedSlot.gameObject.SetActive(true);
                break;
            case PlayerType.Green:
                mGreenSlot.gameObject.SetActive(true);
                break;
        }

        switch (_monster.slotIndex)
        {
            case 0:
                mMonsterSlot0.gameObject.SetActive(true);
                break;
            case 1:
                mMonsterSlot1.gameObject.SetActive(true);
                break;
            case 2:
                mMonsterSlot2.gameObject.SetActive(true); 
                break;
        }
        
        _player.transform.position = mPlayerBattleTrans.position;
        _monster.transform.position = mMonsterBattleTrans.position;

        StartCoroutine(OnBattlePrgress(_player, _monster));
    }

    List<int> mPlayerTpCollect = new List<int>();

    IEnumerator OnBattlePrgress(Player _player, Monster _monster)
    {
        Mng.canvas.stageInfo.MonsterInfoRefresh();

        yield return new WaitForSeconds(2f);

        mPlayerTpCollect.Clear();

        //플레이어 토큰 갯수만큼 주사위 굴린다.
        //플레이어 토큰 중에 가장 큰 수의 순서대로 정렬
        //몬스터 토큰 갯수만큼 빼내어서 합산
        //결과값이 몬스터 AP 숫자보다 커야 공격 작으면 반격 당함

        //kPlayerDiceBoard.Roll(_player.table.TP);

        int monsterAp = 0;
        switch((PlayerType)_player.table.Type)
        {
            case PlayerType.Blue:
                monsterAp = _monster.table.BlueToken;
                break;
            case PlayerType.Purple:
                monsterAp = _monster.table.PurpleToken;
                break;
            case PlayerType.Green:
                monsterAp = _monster.table.GreenToken;
                break;
        }

        int count = Mathf.Min(_player.table.TP, monsterAp);

        for (int i = 0; i < count; i++)
        {
            mPlayerTpCollect.Add(Random.Range(1, 6+1));
        }
        
        mPlayerTpCollect.Sort();


        int totalDamage = 0;
        switch((PlayerType)_player.table.Type)
        {
            case PlayerType.Purple:
                for (int n = 0; n < _monster.table.PurpleToken; n++)
                    totalDamage += mPlayerTpCollect[n];
                break;
            case PlayerType.Green:
                for (int n = 0; n < _monster.table.GreenToken; n++)
                    totalDamage += mPlayerTpCollect[n];
                break;
            case PlayerType.Blue:
                for (int n = 0; n < _monster.table.BlueToken; n++)
                    totalDamage += mPlayerTpCollect[n];
                break;
        }
        

        //if( totalDamage > _monster.table.AP)
            
    }
}
