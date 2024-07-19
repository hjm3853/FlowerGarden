using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    static public MainCanvas instance = null;

    public UIStageInfo stageInfo = null;

    private void Awake()
    {
        instance = this;

        stageInfo = this.GetComponentInChildren<UIStageInfo>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
