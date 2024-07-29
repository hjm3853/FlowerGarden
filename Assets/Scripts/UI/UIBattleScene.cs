using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBattleScene : MonoBehaviour
{
    [Header("플레이어 주사위 슬롯")]
    public GameObject[] kRedDiceArr;
    public GameObject[] kGreenDiceArr;
    public GameObject[] kPurpleDiceArr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayer(Player _player)
    {
        foreach (var p in kRedDiceArr)
            p.SetActive(false);
        foreach (var p in kGreenDiceArr)
            p.SetActive(false);
        foreach (var p in kPurpleDiceArr)
            p.SetActive(false);

        switch (_player.kType){ 
            case EnumDef.PlayerType.Purple:{

                    for (int i = 0; i < _player.table.TP; i++)
                    {
                        kPurpleDiceArr[i].SetActive(true);
                        var t = kPurpleDiceArr[i].GetComponentInChildren<TMP_Text>();
                        t.text = Random.Range(1, 7).ToString();
                    }
                }break;
            case EnumDef.PlayerType.Blue:{
                    
                    for (int i = 0; i < _player.table.TP; i++)
                    {
                        kRedDiceArr[i].SetActive(true);
                        var t = kRedDiceArr[i].GetComponentInChildren<TMP_Text>();
                        t.text = Random.Range(1, 7).ToString();
                    }
                    
                }break;
            case EnumDef.PlayerType.Green:{

                    for (int i = 0; i < _player.table.TP; i++)
                    {
                        kGreenDiceArr[i].SetActive(true);
                        var t = kGreenDiceArr[i].GetComponentInChildren<TMP_Text>();
                        t.text = Random.Range(1, 7).ToString();
                    }
                }break;
        }
    }
}
