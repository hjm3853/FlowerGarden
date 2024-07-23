using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnimation : MonoBehaviour
{
    List<Transform> mNumberQuadList = new List<Transform>();    

    // Start is called before the first frame update
    void Awake()
    {
        mNumberQuadList.Add(transform.Find("1"));
        mNumberQuadList.Add(transform.Find("2"));
        mNumberQuadList.Add(transform.Find("3"));
        mNumberQuadList.Add(transform.Find("4"));
        mNumberQuadList.Add(transform.Find("5"));
        mNumberQuadList.Add(transform.Find("6"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(int _fixNum, Vector3 _from, Vector3 _to, float _dropHeight, float _time)
    {
        mNumberQuadList[_fixNum - 1].transform.localPosition = new Vector3(0f, -0.505f, 0f);
        mNumberQuadList[_fixNum - 1].transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);

        List<int> quadIndexList = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            if (_fixNum - 1 == i)
                continue;

            quadIndexList.Add(i);
        }

        for (int i = 0;i < 5; i++)
        {
            int index = Random.Range(0, quadIndexList.Count);
            int select = quadIndexList[index];
            switch(i)
            {
                case 0:
                    mNumberQuadList[select].transform.localPosition = new Vector3(0f, 0f, -0.505f);
                    mNumberQuadList[select].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case 1:
                    mNumberQuadList[select].transform.localPosition = new Vector3(-0.505f, 0f, -0f);
                    mNumberQuadList[select].transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    break;
                case 2:
                    mNumberQuadList[select].transform.localPosition = new Vector3(0f, 0f, 0.505f);
                    mNumberQuadList[select].transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case 3:
                    mNumberQuadList[select].transform.localPosition = new Vector3(0.505f, 0f, 0.0f);
                    mNumberQuadList[select].transform.localRotation = Quaternion.Euler(0f, 270f, 0f);
                    break;
                case 4:
                    mNumberQuadList[select].transform.localPosition = new Vector3(0f, 0.505f, 0f);
                    mNumberQuadList[select].transform.localRotation = Quaternion.Euler(90f, 180f, -90f);
                    break;
            }

            quadIndexList.Remove(select);
        }

        StartCoroutine(CorPlay(_from, _to, _dropHeight, _time));
    }

    IEnumerator CorPlay(Vector3 _from, Vector3 _to, float _dropHeight, float _time)
    {
        transform.position = _from;

        Vector3 fromDrop = _from;
        fromDrop.y = _from.y + _dropHeight;

        Vector3 land = _from + (_to - _from) * 0.5f;

        float totalTime = _time * 0.5f;
        float currentTime = 0.0f;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(fromDrop, land, currentTime/totalTime);
            transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90f, 0f, 0f), currentTime/totalTime);

            yield return null;
        }

        transform.position = land;

        currentTime = 0.0f;
        while (currentTime < totalTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(land, _to, currentTime / totalTime);
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(180f, 0f, 0f), currentTime / totalTime);

            yield return null;
        }

        transform.position = _to;
        transform.rotation = Quaternion.Euler(180f, 0f, 0f);
    }
}
