using EnumDef;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStageInfo : MonoBehaviour
{
    [Header("���� �÷��̾� ����")]
    public UIPlayerSlotInfo kPurplePlayerSlotInfo;
    [Header("�Ķ� �÷��̾� ����")]
    public UIPlayerSlotInfo kBluePlayerSlotInfo;
    [Header("�ʷ� �÷��̾� ����")]
    public UIPlayerSlotInfo kGreenPlayerSlotInfo;

    [Header("���� Hp")]
    public UIPlayerHpbarInfo kPurplePlayerHpbar;
    [Header("�Ķ� Hp")]
    public UIPlayerHpbarInfo kBluePlayerHpbar;
    [Header("�ʷ� Hp")]
    public UIPlayerHpbarInfo kGreenPlayerHpbar;

    [Header("1�� ���� ���� ����")]
    public UIMonsterSlotInfo kMonsterSlotInfo0;
    [Header("2�� ���� ���� ����")]
    public UIMonsterSlotInfo kMonsterSlotInfo1;
    [Header("3�� ���� ���� ����")]
    public UIMonsterSlotInfo kMonsterSlotInfo2;

    [Header("�ֻ��� ���� ȭ��")]
    public RawImage kDiceScreenImage;

    [Header("���� ���� ��ư")]
    public Button kBattleButton;

    [Header("���� ȭ��")]
    public UIBattleScene kBattleScene;

    // Start is called before the first frame update
    void Start()
    {
        kBattleButton.gameObject.SetActive(false);
        kDiceScreenImage.gameObject.SetActive(false);

        SetPlayerPick(null);
        SetMonsterPick(null);

        kBattleScene.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player _purplePlayer, Player _bluePlayer, Player _greenPlayer)
    {
        kPurplePlayerSlotInfo.Set(_purplePlayer);
        kBluePlayerSlotInfo.Set(_bluePlayer);
        kGreenPlayerSlotInfo.Set(_greenPlayer);

        kPurplePlayerHpbar.Set(_purplePlayer);
        kBluePlayerHpbar.Set(_bluePlayer);
        kGreenPlayerHpbar.Set(_greenPlayer);

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
        kBluePlayerSlotInfo.Pick(false);
        kGreenPlayerSlotInfo.Pick(false);
        
        mSelectPlayer = _player;

        if (mSelectPlayer == null)
            return;

        switch (mSelectPlayer.kType)
        {
            case PlayerType.Purple:
                kPurplePlayerSlotInfo.Pick(true);
                break;
            case PlayerType.Blue:
                kBluePlayerSlotInfo.Pick(true);
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
        kBluePlayerSlotInfo.gameObject.SetActive(false);
        kGreenPlayerSlotInfo.gameObject.SetActive(false);

        kMonsterSlotInfo0.gameObject.SetActive(false);
        kMonsterSlotInfo1.gameObject.SetActive(false);
        kMonsterSlotInfo2.gameObject.SetActive(false);

        kBattleScene.gameObject.SetActive(true);
        //kBattleScene.SetPlayer(mSelectPlayer);
        kBattleButton.gameObject.SetActive(false);

        kPurplePlayerHpbar.gameObject.SetActive(false);
        kBluePlayerHpbar.gameObject.SetActive(false);
        kGreenPlayerHpbar.gameObject.SetActive(false);

        switch (mSelectMonster.slotIndex)
        {
            case 0:{
                    kPurplePlayerHpbar.gameObject.SetActive(true);
                    Vector3 pos = kPurplePlayerHpbar.rectTransform().localPosition;
                    pos.x = 0;
                    kPurplePlayerHpbar.rectTransform().localPosition = pos;
                }break;
            case 1:{
                    kBluePlayerHpbar.gameObject.SetActive(true);
                    Vector3 pos = kBluePlayerHpbar.rectTransform().localPosition;
                    pos.x = 0;
                    kBluePlayerHpbar.rectTransform().localPosition = pos;
                }break;
            case 2:{
                    kGreenPlayerHpbar.gameObject.SetActive(true);
                    Vector3 pos = kGreenPlayerHpbar.rectTransform().localPosition;
                    pos.x = 0;
                    kGreenPlayerHpbar.rectTransform().localPosition = pos;
                }break;
        }        

        Mng.play.kStage.SetBattle(mSelectPlayer, mSelectMonster);        
    }

    public void MonsterInfoRefresh()
    {
        switch (mSelectMonster.slotIndex)
        {
            case 0:
                kMonsterSlotInfo0.gameObject.SetActive(true);
                kMonsterSlotInfo0.Set(mSelectMonster);
                break;
            case 1:
                kMonsterSlotInfo1.gameObject.SetActive(true);
                kMonsterSlotInfo1.Set(mSelectMonster);
                break;
            case 2:
                kMonsterSlotInfo2.gameObject.SetActive(true);
                kMonsterSlotInfo2.Set(mSelectMonster);
                break;
        }
    }
}
