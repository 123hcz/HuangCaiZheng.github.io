using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;
using SonicBloom.Koreo.Players;
using UnityEngine.SceneManagement;

public class RhythmGameController : MonoBehaviour {

    [Tooltip("用于目标生成的轨道的事件对应ID")]
    [EventID]
    public string eventID;

    //音符速度
    public float noteSpeed = 1;

    [Tooltip("音符命中区间窗口（音符被命中的难度，单位：ms）")]
    [Range(8f,300f)]
    public float hitWindowRangeInMS;

    //以Unity为单位来访问当前命中窗口的大小
    public float WindowSizeInUnits
    {
        get
        {
            return noteSpeed * (hitWindowRangeInMS * 0.001f);
        }
    }

    //在音乐样本中的命中窗口
    int hitWindowRangeInSamples;

    public int HitWindowSampleWidth
    {
        get
        {
            return hitWindowRangeInSamples;
        }
    }

    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }

    Stack<NoteObject> noteObjectPool = new Stack<NoteObject>();

    public Stack<GameObject> downEffectObjectPool = new Stack<GameObject>();

    public Stack<GameObject> hitEffectObjectPool = new Stack<GameObject>();

    public Stack<GameObject> hitLongNoteEffectObjectPool = new Stack<GameObject>();

    

    //预制体资源
    //public GameObject noteObject;
    //音符
    public NoteObject noteObject;
    //按下特效
    public GameObject downEffectGo;
    //击中音符特效
    public GameObject hitEffectGo;
    //击中长音符特效
    //public GameObject hitLongNoteEffectGo;

    //引用
    Koreography playingKoreo;

    public AudioSource audioCom;

    public List<LaneController> noteLanes = new List<LaneController>();

    SimpleMusicPlayer simpleMusicPlayer;

    public Transform simpleMusciPlayerTrans;

    //public GameObject gameOverUIGo;

    //其他
    [Tooltip("开始播放音频之前提供的时间量（以秒为单位），也就是提前调用时间")]
    public float leadInTime;
    //音频播放之前的剩余时间量
    float leadInTimeLeft;
    //音乐开始之前的倒计时器
    float timeLeftToPlay;
    //当前的采样时间，包括任何必要的延迟
    public int DelayedSampleTime
    {
        get
        {
            return playingKoreo.GetLatestSampleTime()-(int)(SampleRate*leadInTimeLeft);
        }
    }

    float hideHitLevelImageTimeVal;

    public Animator hitLevelImageAnim;

    public Animator comboTextAnim;

    public int comboNum;

    public int score;

    public int hp = 10;

    public bool isPauseState;

    public bool gameStart;

    //UI

    public Slider slider;

    public Text scoreText;

    public Image hitLevelImage;

    public Text comboText;

    public GameObject gameOverUI;

    //资源
    public Sprite[] hitLevelSprites;

    public Koreography kgy;

    // Use this for initialization
    void Start()
    {
        InitializeLeadIn();
        simpleMusicPlayer = simpleMusciPlayerTrans.GetComponent<SimpleMusicPlayer>();
        simpleMusicPlayer.LoadSong(kgy, 0, false);
        // 初始化所有音轨.
        for (int i = 0; i < noteLanes.Count; ++i)
        {
            noteLanes[i].Initialize(this);
        }

        // 初始化事件。
        playingKoreo = Koreographer.Instance.GetKoreographyAtIndex(0);

        // 获取Koreography中的所有事件。
        KoreographyTrackBase rhythmTrack = playingKoreo.GetTrackByID(eventID);//获取事件轨迹
        List<KoreographyEvent> rawEvents = rhythmTrack.GetAllEvents();//获取所有事件
                                                                      //KoreographyEvent rawEvent = rhythmTrack.GetEventAtStartSample(2419200);
                                                                      //rawEvent.
        for (int i = 0; i < rawEvents.Count; ++i)
        {
            //KoreographyEvent  基础Koreography事件定义。 每个事件实例都可以携带一个
            //有效载荷 事件可以跨越一系列样本，也可以绑定到一个样本。 样品
            //值（开始/结束）在“采样时间”范围内，* NOT *绝对采样位置。
            //确保查询/比较在TIME而不是DATA空间中发生。
            KoreographyEvent evt = rawEvents[i];
            int noteID = evt.GetIntValue();//获取每个事件对应的字符串

            // Find the right lane.  遍历所有音轨
            for (int j = 0; j < noteLanes.Count; ++j)
            {
                LaneController lane = noteLanes[j];
                if (noteID > 6)
                {
                    noteID = noteID - 6;
                    if (noteID > 6)
                    {
                        noteID = noteID - 6;
                    }
                }
                if (lane.DoesMatch(noteID))
                {
                    //事件对应的字符串与某个音轨对应字符串匹配，则把该事件添加到该音轨
                    // Add the object for input tracking.
                    lane.AddEventToLane(evt);

                    // Break out of the lane searching loop.
                    break;
                }
            }
        }
        //SampleRate采样率，在音频资源里有。
        //命中窗口宽度，采样率*0.001*命中时长
        hitWindowRangeInSamples = (int)(0.001f * hitWindowRangeInMS * SampleRate);
    }

    // Update is called once per frame
    void Update () {

        if (isPauseState)
        {
            return;
        }

        if (timeLeftToPlay>0)
        {
            timeLeftToPlay -= Time.unscaledDeltaTime;

            if (timeLeftToPlay<=0)
            {
                audioCom.Play();
                gameStart = true;
                timeLeftToPlay = 0;
            }
        }

        //倒数我们的引导时间
        if (leadInTimeLeft>0)
        {
            leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0);
        }

        if (hitLevelImage.gameObject.activeSelf)
        {
            if (hideHitLevelImageTimeVal>0)
            {
                hideHitLevelImageTimeVal -= Time.deltaTime;
            }
            else
            {
                HideComboNumText();
                HideHitLevelImage();
            }
        }

        if (gameStart)
        {
            if (!simpleMusicPlayer.IsPlaying)
            {
                gameOverUI.SetActive(true);
            }
        }
        
	}

    /// <summary>
    /// 初始化引导时间
    /// </summary>
    void InitializeLeadIn()
    {
        if (leadInTime>0)
        {
            leadInTimeLeft = leadInTime;
            timeLeftToPlay = leadInTime;
        }
        else
        {
            audioCom.Play();
        }
    }


    /// <summary>
    /// 对象池有关
    /// </summary>
    /// <returns></returns>
    //从池中取对象的方法
    public NoteObject GetFreshNoteObject()
    {
        NoteObject retObj;

        if (noteObjectPool.Count>0)
        {
            retObj = noteObjectPool.Pop();
        }
        else
        {
            //资源源
            //retObj = Instantiate(noteObject);
            retObj = Instantiate(noteObject);
        }

        retObj.transform.position = Vector3.one*2;
        retObj.gameObject.SetActive(true);
        //retObj.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }

    //将音符对象放回对象池
    public void ReturnNoteObjectToPool(NoteObject obj)
    {
        if (obj!=null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);
            noteObjectPool.Push(obj);
        }
    }

    public GameObject GetFreshEffectObject(Stack<GameObject> stack,GameObject effectObject)
    {
        GameObject effectGo;

        if (stack.Count>0)
        {
            effectGo=stack.Pop();
        }
        else
        {
            effectGo = Instantiate(effectObject);
        }

        effectGo.SetActive(true);

        return effectGo;
    }

    public void ReturnEffectGoToPool(GameObject effectGo,Stack<GameObject> stack)
    {
        if (effectGo!=null)
        {
            effectGo.gameObject.SetActive(false);
            stack.Push(effectGo);
        }
    }

    //显示命中等级对应的图片
    public void ChangeHitLevelSprite(int hitLevel)
    {
        hideHitLevelImageTimeVal = 1;
        hitLevelImage.sprite = hitLevelSprites[hitLevel];
        hitLevelImage.SetNativeSize();
        hitLevelImage.gameObject.SetActive(true);
        hitLevelImageAnim.SetBool("IsNoteHittable",true);
        if (comboNum>=5)
        {
            comboText.gameObject.SetActive(true);
            comboText.text = comboNum.ToString();
            comboTextAnim.SetBool("IsNoteHittable",true);

        }
        //hitLevelImageAnim.Play("UIAnimation");
    }

    private void HideHitLevelImage()
    {
        hitLevelImage.gameObject.SetActive(false);
    }

    public void HideComboNumText()
    {
        comboText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(int addNum)
    {
        score += addNum;
        scoreText.text = score.ToString();
    }

    public void UpdateHP()
    {
        hp = hp - 2;
        slider.value = (float)hp / 10;
        if (hp==0)
        {
            isPauseState = true;
            gameOverUI.SetActive(true);
            PauseMusic();
        }
    }

    //游戏的开始与暂停
    public void PauseMusic()
    {
        if (!gameStart)
        {
            return;
        }
        simpleMusicPlayer.Pause();
    }

    public void PlayMusic()
    {
        if (!gameStart)
        {
            return;
        }
        simpleMusicPlayer.Play();
    }

    public void Replay()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
