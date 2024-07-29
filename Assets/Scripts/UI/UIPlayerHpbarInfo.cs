using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpbarInfo : MonoBehaviour
{
    [Header("플레이어 Hp(text)")]
    public TMP_Text kCurrentHpTxt;
    [Header("플레이어 Hp(sprite)")]
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
        kCurrentHpTxt.text = $"캐릭터 Hp : {_player.hp}";
        kFillHpSpr.fillAmount = (float)_player.hp / (float)_player.table.Hp;
    }
}
