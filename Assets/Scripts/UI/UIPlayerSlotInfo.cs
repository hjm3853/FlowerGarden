using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerSlotInfo : MonoBehaviour
{
    [Header("�÷��̾� �ֻ��� ����")]
    public TMP_Text kDiceCnt;

    [Header("�÷��̾� ����")]
    public GameObject kPick;

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
        kDiceCnt.text = _player.table.Dice.ToString();

        Vector3 pos = Camera.main.WorldToScreenPoint(_player.GetDiceTransform().position);
        transform.position = pos;
    }

    public void Pick(bool _isPick)
    {
        kPick.gameObject.SetActive(_isPick);
    }
}
