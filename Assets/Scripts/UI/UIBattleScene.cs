using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleScene : MonoBehaviour
{
    [Header("�÷��̾� ���ط�")]
    public TMP_Text kPlayerDamageTxt;
    [Header("���� ���ط�")]
    public TMP_Text kMonsterDamageTxt;
    [Header("����")]
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
