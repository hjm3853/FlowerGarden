using EnumDef;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageInfo : MonoBehaviour
{
    [Header("플레이어 총 체력")]
    public TMP_Text kPlayerTotalHp;

    [Header("보라 플레이어 정보")]
    public UIPlayerSlotInfo kPurplePlayerSlotInfo;
    [Header("빨강 플레이어 정보")]
    public UIPlayerSlotInfo kRedPlayerSlotInfo;
    [Header("초록 플레이어 정보")]
    public UIPlayerSlotInfo kGreenPlayerSlotInfo;

    [Header("1번 슬롯 몬스터 정보")]
    public UIMonsterSlotInfo kMonsterSlotInfo0;
    [Header("2번 슬롯 몬스터 정보")]
    public UIMonsterSlotInfo kMonsterSlotInfo1;
    [Header("3번 슬롯 몬스터 정보")]
    public UIMonsterSlotInfo kMonsterSlotInfo2;    

    [Header("전투 실행 버튼")]
    public Button kBattleButton;

    [Header("전투 화면")]
    public UIBattleScene kBattleScene;

    // Start is called before the first frame update
    void Start()
    {
        kBattleButton.gameObject.SetActive(false);

        SetPlayerPick(null);
        SetMonsterPick(null);

        kBattleScene.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player _purplePlayer, Player _redPlayer, Player _greenPlayer)
    {
        kPurplePlayerSlotInfo.Set(_purplePlayer);
        kRedPlayerSlotInfo.Set(_redPlayer);
        kGreenPlayerSlotInfo.Set(_greenPlayer);

        kPlayerTotalHp.text = (_purplePlayer.table.Hp + _redPlayer.table.Hp + _greenPlayer.table.Hp).ToString();
    }

    public void SetMonster(Monster _mon0, Monster _mon1, Monster _mon2)
    {
        kMonsterSlotInfo0.Set(_mon0);
        kMonsterSlotInfo1.Set(_mon1);
        kMonsterSlotInfo2.Set(_mon2);
    }

    Player mSelectPlayer = null;
    Monster mSelectMonster = null;

    public void SetPlayerPick(Player _player)
    {
        kPurplePlayerSlotInfo.Pick(false);
        kRedPlayerSlotInfo.Pick(false);
        kGreenPlayerSlotInfo.Pick(false);
        
        mSelectPlayer = _player;

        if (mSelectPlayer == null)
            return;

        switch (mSelectPlayer.kType)
        {
            case PlayerType.Purple:
                kPurplePlayerSlotInfo.Pick(true);
                break;
            case PlayerType.Red:
                kRedPlayerSlotInfo.Pick(true);
                break;            
            case PlayerType.Green:
                kGreenPlayerSlotInfo.Pick(true);
                break;
        }

        if(mSelectMonster != null)
        {
            kBattleButton.gameObject.SetActive(true);
        }
    }

    public void SetMonsterPick(Monster _mon)
    {
        kMonsterSlotInfo0.Pick(false);
        kMonsterSlotInfo1.Pick(false);
        kMonsterSlotInfo2.Pick(false);

        mSelectMonster = _mon;

        if (mSelectMonster == null)
            return;

        switch (mSelectMonster.slotIndex)
        {            
            case 0:
                kMonsterSlotInfo0.Pick(true);
                break;
            case 1:
                kMonsterSlotInfo1.Pick(true);
                break;
            case 2:
                kMonsterSlotInfo2.Pick(true);
                break;
        }

        if (mSelectPlayer != null)
        {
            kBattleButton.gameObject.SetActive(true);
        }
    }

    public void OnBattleButtonClick()
    {
        kPurplePlayerSlotInfo.gameObject.SetActive(false);
        kRedPlayerSlotInfo.gameObject.SetActive(false);
        kGreenPlayerSlotInfo.gameObject.SetActive(false);

        kMonsterSlotInfo0.gameObject.SetActive(false);
        kMonsterSlotInfo1.gameObject.SetActive(false);
        kMonsterSlotInfo2.gameObject.SetActive(false);

        kBattleScene.gameObject.SetActive(true);
        kBattleScene.SetPlayer(mSelectPlayer);

        Mng.play.kStage.SetBattle(mSelectPlayer, mSelectMonster);
    }
}
