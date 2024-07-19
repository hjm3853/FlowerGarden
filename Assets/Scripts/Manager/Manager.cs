using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
	static public Manager Instance = null;

	[Header("테이블 매니저")] public TableManager kTableManager;
    [Header("플레이 매니저")] public PlayManager kPlayManager;
    [Header("사운드 매니저")] public SoundManager kSoundManager;
    [Header("캔버스 매니저")] public MainCanvas kCanvasManager;

    // Start is called before the first frame update
    void Awake()
	{
		Instance = this;

		//Caching.ClearCache();
	}

	IEnumerator Start()
	{
        if (kTableManager != null)
        {
            kTableManager.gameObject.SetActive(true);
            yield return null;
        }

        if (kTableManager != null)
        {
            kCanvasManager.enabled = true;
            yield return null;
        }


        if (kSoundManager != null)
        {
            kSoundManager.gameObject.SetActive(true);
            yield return null;
        }

        if (kPlayManager != null)
        {
            kPlayManager.gameObject.SetActive(true);
            yield return null;
        }

        /*
                if (kTableManager != null)
                {
                    GameObject go = Instantiate(kAntiCheatManager);
                    go.transform.parent(transform);
                    yield return null;
                }

                yield return ProcessWaiting(DoneCategory.AntiCheat_Init);

                if (kAddressableManager != null)
                {
                    GameObject go = Instantiate(kAddressableManager);
                    go.transform.parent(transform);
                    yield return null;
                }
        */
        yield return null;
    }

	private void Update()
	{
	}
}