using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class LaneController : MonoBehaviour {

    RhythmGameController gameController;

    public KinectControl KinectInput;

    [Tooltip("此音轨使用的键盘按键")]
    public KeyCode keyboardButton;

    [Tooltip("此音轨对应事件的编号")]
    public int laneID;

    public List<bool> traikcs = new List<bool>();
    public bool RightInputDonw;
    public bool LeftInputDonw;
    public bool IsDwon;
    public bool InputDown;


    //对“目标”位置的键盘按下的视觉效果
    public Transform targetVisuals;

    //上下边界
    public Transform targetTopTrans;
    public Transform targetBottomTrans;

    //包含在此音轨中的所有事件列表
    List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();

    //包含此音轨当前活动的所有音符对象的队列
    Queue<NoteObject> trackedNotes = new Queue<NoteObject>();

    //检测此音轨中的生成的下一个事件的索引
    int pendingEventIdx = 0;

    public GameObject downVisual;

    //音符移动的目标位置
    public Vector3 TargetPosition
    {
        get
        {
            return transform.position;
        }
    }

    public bool hasLongNote;

    public float timeVal = 0;

    public GameObject longNoteHitEffectGo;

    private void Start()
    {
        RightInputDonw = false;
        LeftInputDonw = false;
        IsDwon = false;
    }

    // Update is called once per frame
    void Update() {

        if (gameController.isPauseState)
        {
            return;
        }
        //清除无效音符
        while (trackedNotes.Count>0&&trackedNotes.Peek().IsNoteMissed())
        {
            if (trackedNotes.Peek().isLongNoteEnd)
            {
                hasLongNote = false;
                timeVal = 0;
                downVisual.SetActive(false);
                longNoteHitEffectGo.SetActive(false);
            }
            gameController.comboNum = 0;
            gameController.HideComboNumText();
            gameController.ChangeHitLevelSprite(0);
            gameController.UpdateHP();
            trackedNotes.Dequeue();
        }

        //检测新音符的产生
        CheckSpawnNext();
        //检测玩家的输入
        traikcs[1] = KinectInput.flagtrak1;
        traikcs[2] = KinectInput.flagtrak2;
        traikcs[3] = KinectInput.flagtrak3;
        traikcs[4] = KinectInput.flagtrak4;
        traikcs[5] = KinectInput.flagtrak5;
        traikcs[6] = KinectInput.flagtrak6;
        RightInputDonw = !KinectInput.RightHandOpen;
        //LeftInputDonw = !KinectInput.LeftHandOpen;
        if (laneID >= 1 && laneID <= 3)
        {
            InputDown = !KinectInput.LeftHandOpen;
        }
        else {
            InputDown = !KinectInput.RightHandOpen;
        }

        if (Input.GetKeyDown(keyboardButton) || (InputDown && traikcs[laneID]))
        {
            CheckNoteHit();
            downVisual.SetActive(true);
            IsDwon = true;
        }
        else if (Input.GetKey(keyboardButton))
        {
            //检测长音符
            if (hasLongNote)
            {
                if (timeVal>=0.15f)
                {
                    //显示命中等级（Great Perfect）
                    if (!longNoteHitEffectGo.activeSelf)
                    {
                        gameController.ChangeHitLevelSprite(2);
                        CreateHitLongEffect();
                    }
                    timeVal = 0;
                }
                else
                {
                    timeVal += Time.deltaTime;
                }
            }
        }
        else if (Input.GetKeyUp(keyboardButton)||(IsDwon && !traikcs[laneID]))
        {
            IsDwon = false;
            downVisual.SetActive(false);
            //检测长音符
            if (hasLongNote)
            {
                longNoteHitEffectGo.SetActive(false);
                CheckNoteHit();
            }
        }

    }

    //初始化
    public void Initialize(RhythmGameController controller)
    {
        gameController = controller;
    }

    //检测事件是否匹配当前编号的音轨
    public bool DoesMatch(int noteID)
    {
        return noteID == laneID;
    }

    //如果匹配，则把当前事件添加进音轨所持有的事件列表
    public void AddEventToLane(KoreographyEvent evt)
    {
        laneEvents.Add(evt);
    }

    //音符在音谱上产生的位置偏移量
    int GetSpawnSampleOffset()
    {
        //出生位置与目标点的位置
        float spawnDistToTarget = targetTopTrans.position.z - transform.position.z;

        //到达目标点的时间
        float spawnPosToTargetTime = spawnDistToTarget / gameController.noteSpeed;

        return (int)spawnPosToTargetTime * gameController.SampleRate;
    }

    //检测是否生成下一个新音符
    void CheckSpawnNext()
    {
        int samplesToTarget = GetSpawnSampleOffset();

        int currentTime = gameController.DelayedSampleTime;

        while (pendingEventIdx < laneEvents.Count
            && laneEvents[pendingEventIdx].StartSample < currentTime + samplesToTarget)
        {
            KoreographyEvent evt = laneEvents[pendingEventIdx];
            int noteNum = evt.GetIntValue();
            NoteObject newObj = gameController.GetFreshNoteObject();
            bool isLongNoteStart = false;
            bool isLongNoteEnd = false;
            if (noteNum > 6)
            {
                isLongNoteStart = true;
                noteNum = noteNum - 6;
                if (noteNum > 6)
                {
                    isLongNoteEnd = true;
                    isLongNoteStart = false;
                    noteNum = noteNum - 6;
                }
            }
            newObj.Initialize(evt, noteNum, this, gameController, isLongNoteStart, isLongNoteEnd);
            trackedNotes.Enqueue(newObj);
            pendingEventIdx++;
        }
    }

    /// <summary>
    /// 生成特效的有关方法
    /// </summary>
    void CreateDownEffect()
    {
        GameObject downEffectGo = gameController.GetFreshEffectObject(gameController.downEffectObjectPool, gameController.downEffectGo);
        downEffectGo.transform.position = targetVisuals.position;
    }

    void CreateHitEffect()
    {
        GameObject hitEffectGo = gameController.GetFreshEffectObject(gameController.hitEffectObjectPool, gameController.hitEffectGo);
        hitEffectGo.transform.position = targetVisuals.position;
    }

    void CreateHitLongEffect()
    {
        longNoteHitEffectGo.SetActive(true);
        longNoteHitEffectGo.transform.position = targetVisuals.position;
    }

    //检测是否有击中音符对象
    //如果是，它将执行命中并删除
    public void CheckNoteHit()
    {
        if (!gameController.gameStart)
        {
            CreateDownEffect();
            return;
        }
        if (trackedNotes.Count>0)
        {
            NoteObject noteObject = trackedNotes.Peek();
            if (noteObject.hitOffset>-6000)
            {
                trackedNotes.Dequeue();
                int hitLevel= noteObject.IsNoteHittable();
                gameController.ChangeHitLevelSprite(hitLevel);
                if (hitLevel>0)
                {
                    //更新分数
                    gameController.UpdateScoreText(100 * hitLevel);
                    if (noteObject.isLongNote)
                    {
                        hasLongNote = true;
                        CreateHitLongEffect();
                    }
                    else if (noteObject.isLongNoteEnd)
                    {
                        hasLongNote = false;
                    }
                    else
                    {
                        CreateHitEffect();
                    }

                    //增加连接数
                    gameController.comboNum++;
                }
                else
                {
                    //未击中
                    //减少玩家HP
                    gameController.UpdateHP();
                    //断掉玩家命中连接数
                    gameController.HideComboNumText();
                    gameController.comboNum = 0;
                }
                noteObject.OnHit();
            }
            else
            {
                CreateDownEffect();
            }
        }
        else
        {
            CreateDownEffect();
        }
    }
}
