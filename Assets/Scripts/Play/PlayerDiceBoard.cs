using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiceBoard : MonoBehaviour
{
    List<DiceAnimation> kModelList = new List<DiceAnimation>();
    List<Transform> kSlotList = new List<Transform>();

    [Header("주사위 무작위 배치 : 가로 범위")]    
    public float kDiceRandomHorizon = 0.5f;
    [Header("주사위 무작위 배치 : 세로 범위")]
    public float kDiceRandomVertical = 0.5f;

    [Header("주사위가 떨어지는 높이")]
    public float kDiceDropHeight = 1f;
    [Header("주사위가 전진하는 거리")]
    public float kDiceForwardDist = 2f;
    [Header("주사위 애니메이션 시간")]
    public float kDiceAnimationTime = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        var models = transform.Find("Dice/Models").gameObject;
        for(int i = 0; i < models.transform.childCount; i++)
        {
            kModelList.Add(models.transform.GetChild(i).GetComponent<DiceAnimation>());
        }

        var slots = transform.Find("Dice/Slots").gameObject;
        for (int i = 0; i < slots.transform.childCount; i++)
        {
            kSlotList.Add(slots.transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<int> mDiceIndexList = new List<int>();

    [Header("주사위 테스트 갯수")]
    public int kTestCount = 5;
    
    [Button("주사위")]
    public void Roll(bool _isTest = true, List<int> _fixedDiceList = null)
    {
        mDiceIndexList.Clear();

        for(int i = 0;i < kModelList.Count; i++)
        {
            kModelList[i].gameObject.SetActive(false);
            mDiceIndexList.Add(i);
        }

        int count = _isTest ? kTestCount : _fixedDiceList.Count;

        for (int i = 0; i < count; i++)
        {
            var select = Random.Range(0, mDiceIndexList.Count);

            var index = mDiceIndexList[select];

            var dice = kModelList[index];
            dice.gameObject.SetActive(true);
            
            float addX = Random.Range(-kDiceRandomHorizon * 0.5f, kDiceRandomHorizon * 0.5f);
            float addZ = Random.Range(-kDiceRandomVertical * 0.5f, kDiceRandomVertical * 0.5f);
            dice.transform.position = kSlotList[index].transform.position + new Vector3(addX, 0f, addZ);

            dice.transform.forward = Vector3.forward;

            var from = dice.transform.position;
            var to = dice.transform.position + dice.transform.forward * kDiceForwardDist;

            if(_isTest == true)
                dice.Play(Random.Range(1, 6+1), from, to, kDiceDropHeight, kDiceAnimationTime);
            else
                dice.Play(_fixedDiceList[i], from, to, kDiceDropHeight, kDiceAnimationTime);

            mDiceIndexList.Remove(index);
        }

        Mng.canvas.stageInfo.kDiceScreenImage.gameObject.SetActive(true);
    }
}
