using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHpbarInfo : MonoBehaviour
{
    [Header("�÷��̾� Hp(text)")]
    public TMP_Text kCurrentHpTxt;
    
    [Header("�÷��̾� Hp(sprite)-75% �̻�")]
    public Image kHpSpr75;
    [Header("�÷��̾� Hp(sprite)-50% �̻�")]
    public Image kHpSpr50;
    [Header("�÷��̾� Hp(sprite)-25% �̻�")]
    public Image kHpSpr25;
    [Header("�÷��̾� Hp(sprite)-0% �̻�")]
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
