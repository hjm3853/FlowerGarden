using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpbarInfo : MonoBehaviour
{
    [Header("�÷��̾� Hp(text)")]
    public TMP_Text kCurrentHpTxt;
    [Header("�÷��̾� Hp(sprite)")]
    public Image kFillHpSpr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Player _player)
    {
        kCurrentHpTxt.text = $"ĳ���� Hp : {_player.hp}";
        kFillHpSpr.fillAmount = (float)_player.hp / (float)_player.table.Hp;
    }
}
