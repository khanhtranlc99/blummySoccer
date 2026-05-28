using System;
using UnityEngine;

public struct MoveAction
{
    private Transform m_Transf;

    private float m_TimeCount;

    private float m_Time;

    private Vector3 m_startPosition;

    private Vector3 m_targetPosition;

    private bool m_disableOnAnimFinish;

    private Action m_EndAction;

    private float m_Delay;

    private bool m_WorldSpace;

    public MoveAction(Transform transf, float time, Vector3 startPosition, Vector3 endPosition, bool disableOnAnimFinish, Action endAction, bool worldSpace = true, float delay = 0f)
    {
        m_Time = time;
        m_Transf = transf;
        m_TimeCount = 0f;
        m_startPosition = startPosition;
        m_targetPosition = endPosition;
        m_disableOnAnimFinish = disableOnAnimFinish;
        m_EndAction = endAction;
        m_WorldSpace = worldSpace;
        m_Delay = delay;
    }

    public void Init(Transform transf, float time, Vector3 startPosition, Vector3 endPosition, bool disableOnAnimFinish, Action endAction, bool worldSpace = true, float delay = 0f)
    {
        m_Time = time;
        m_Transf = transf;
        m_TimeCount = 0f;
        m_startPosition = startPosition;
        m_targetPosition = endPosition;
        m_disableOnAnimFinish = disableOnAnimFinish;
        m_EndAction = endAction;
        m_WorldSpace = worldSpace;
        m_Delay = delay;
    }
    private void SetPos(Vector3 pos)
    {
        if (m_WorldSpace)
        {
            m_Transf.position = pos;
        }
        else
        {
            m_Transf.localPosition = pos;
        }
    }

    public bool Update(float deltaTime)
    {
        m_TimeCount += deltaTime;
        if (m_TimeCount < m_Delay)
        {
            return false;
        }
        Vector3 pos = Vector3.Lerp(m_startPosition, m_targetPosition, (m_TimeCount - m_Delay) / m_Time);
        SetPos(pos);
        if (!m_Transf.gameObject.activeSelf)
        {
            m_Transf.gameObject.SetActive(value: true);
        }
        if (m_TimeCount - m_Delay > m_Time)
        {
            if (m_disableOnAnimFinish)
            {
                m_Transf.gameObject.SetActive(value: false);
            }
            if (m_EndAction != null)
            {
                m_EndAction();
            }
            m_Transf = null;
            m_EndAction = null;
            return true;
        }
        return false;
    }

    public void OnRecycled()
    {
        m_Transf = null;
    }
}
