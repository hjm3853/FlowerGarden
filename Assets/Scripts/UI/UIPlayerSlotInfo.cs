using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerSlotInfo : MonoBehaviour
{
    [Header("플레이어 토큰 포인트")]
    public TMP_Text kDiceCnt;
    [Header("플레이어 이름")]
    public TMP_Text kName;

    [Header("플레이어 선택")]
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
        kDiceCnt.text = $"TP : {_player.table.TP}";
        kName.text = _player.table.Name;

        Vector3 pos = Camera.main.WorldToScreenPoint(_player.GetDiceTransform().position);
        transform.position = pos;
    }

    public void Pick(bool _isPick)
    {
        kPick.gameObject.SetActive(_isPick);
    }
}
