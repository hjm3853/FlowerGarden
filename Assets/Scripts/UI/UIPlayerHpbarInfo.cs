using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpbarInfo : MonoBehaviour
{
    [Header("플레이어 Hp(text)")]
    public TMP_Text kCurrentHpTxt;
    
    [Header("플레이어 Hp(sprite)-75% 이상")]
    public Image kHpSpr75;
    [Header("플레이어 Hp(sprite)-50% 이상")]
    public Image kHpSpr50;
    [Header("플레이어 Hp(sprite)-25% 이상")]
    public Image kHpSpr25;
    [Header("플레이어 Hp(sprite)-0% 이상")]
    public Image kHpSpr00;

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
        kCurrentHpTxt.text = $"Hp : {_player.hp}";

        kHpSpr75.enabled = false;
        kHpSpr50.enabled = false;
        kHpSpr25.enabled = false;
        kHpSpr00.enabled = false;

        float ratio = (float)_player.hp / (float)_player.table.Hp;

        if (ratio >= 0.75f)
        {
            kHpSpr75.enabled = true;
        }
        else if(ratio >= 0.5f)
        {
            kHpSpr50.enabled = true;
        }
        else if (ratio >= 0.25f)
        {
            kHpSpr25.enabled = true;
        }
        else
        {
            kHpSpr00.enabled = true;
        }
    }
}
