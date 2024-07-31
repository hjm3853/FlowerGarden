using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleScene : MonoBehaviour
{
    [Header("플레이어 피해량")]
    public TMP_Text kPlayerDamageTxt;
    [Header("몬스터 피해량")]
    public TMP_Text kMonsterDamageTxt;
    [Header("라운드")]
    public TMP_Text kRoundTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        kMonsterDamageTxt.text = "";
        kPlayerDamageTxt.text = "";
    }

    public void SetPlayerDamage(int _damage)
    {
        kMonsterDamageTxt.text = "";
        kPlayerDamageTxt.text = $"-{_damage.ToString()}";
    }

    public void SetMonsterDamage(int _damage)
    {
        kPlayerDamageTxt.text = "";
        kMonsterDamageTxt.text = $"-{_damage.ToString()}";
    }

    public void SetRound(int _count)
    {
        kRoundTxt.text = _count.ToString();
    }
}
