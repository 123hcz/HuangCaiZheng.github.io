using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

public class NoteObject : MonoBehaviour {

    public SpriteRenderer visuals;

    public Sprite[] noteSprites;

    KoreographyEvent trackedEvent;

    public bool isLongNote;

    public bool isLongNoteEnd;

    LaneController laneController;

    RhythmGameController gameController;

    public int hitOffset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameController.isPauseState)
        {
            return;
        }

        UpdatePosition();
        GetHitOffset();
        if (transform.position.z<=laneController.targetBottomTrans.position.z)
        {
            gameController.ReturnNoteObjectToPool(this);
            ResetNote();
        }
    }

    //初始化方法
    public void Initialize(KoreographyEvent evt,int noteNum,LaneController laneCont,
        RhythmGameController gameCont,bool isLongStart,bool isLongEnd)
    {
        trackedEvent = evt;
        laneController = laneCont;
        gameController = gameCont;
        isLongNote = isLongStart;
        isLongNoteEnd = isLongEnd;
        int spriteNum = noteNum;
        if (isLongNote)
        {
            spriteNum+=6;
        }
        else if (isLongNoteEnd)
        {
            spriteNum += 12;
        }
        visuals.sprite = noteSprites[spriteNum - 1];
    }

    //将Note对象重置
    private void ResetNote()
    {
        trackedEvent = null;
        laneController = null;
        gameController = null;
    }

    //返回对象池
    void ReturnToPool()
    {
        gameController.ReturnNoteObjectToPool(this);
        ResetNote();
    }

    //击中音符对象
    public void OnHit()
    {
        ReturnToPool();
    }

    //更新位置的方法
    void UpdatePosition()
    {
        Vector3 pos = laneController.TargetPosition;

        pos.z -= (gameController.DelayedSampleTime - trackedEvent.StartSample) / (float)gameController.SampleRate * gameController.noteSpeed;

        transform.position = pos;
    }

    void GetHitOffset()
    {
        int curTime = gameController.DelayedSampleTime;
        int noteTime = trackedEvent.StartSample;
        int hitWindow = gameController.HitWindowSampleWidth;
        hitOffset = hitWindow - Mathf.Abs(noteTime-curTime);
    }

    //当前音符是否已经Miss
    public bool IsNoteMissed()
    {
        bool bMissed = true;
        if (enabled)
        {
            int curTime = gameController.DelayedSampleTime;
            int noteTime = trackedEvent.StartSample;
            int hitWindow = gameController.HitWindowSampleWidth;

            bMissed = curTime - noteTime > hitWindow;
        }
        return bMissed;
    }

    //音符的命中等级
    public int IsNoteHittable()
    {
        int hitLevel = 0;
        if (hitOffset>=0)
        {
            if (hitOffset>=2000&&hitOffset<=9000)
            {
                hitLevel = 2;
            }
            else
            {
                hitLevel = 1;
            }
        }
        else
        {
            this.enabled = false;
        }

        return hitLevel;
    }
}
