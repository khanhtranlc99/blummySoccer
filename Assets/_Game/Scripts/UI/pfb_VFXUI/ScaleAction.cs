using UnityEngine;

public struct ScaleAction
{
    private AnimationCurve m_YCurve;

    private AnimationCurve m_XCurve;

    private AnimationCurve m_ZCurve;

    private float m_Time;

    private float m_TimeCount;

    private float m_DelayTime;

    private Transform m_Target;
    private float m_ValueCurveX;
    private float m_ValueCurveY;

    public void Init(float time, AnimationCurve xcurve, AnimationCurve ycurve, AnimationCurve zcurve, Transform target, float delayTime, float ValueCurveX = 1, float ValueCurveY = 1)
    {
        m_Time = time;
        m_TimeCount = 0f;
        m_Target = target;
        m_XCurve = xcurve;
        m_YCurve = ycurve;
        m_ZCurve = zcurve;
        m_DelayTime = delayTime;
        m_ValueCurveX = ValueCurveX;
        m_ValueCurveY = ValueCurveY;
    }

    public bool Update(float deltaTime)
    {
        m_TimeCount += deltaTime;
        if (m_TimeCount < m_DelayTime)
        {
            return false;
        }
        Vector3 localScale = m_Target.localScale;
        float num = (m_TimeCount - m_DelayTime) / m_Time;
        if (num >= 1f)
        {
            m_Target.localScale = new Vector3((m_XCurve != null) ? m_XCurve.Evaluate(1f) * m_ValueCurveX : localScale.x, (m_YCurve != null) ? m_YCurve.Evaluate(1f) * m_ValueCurveY: localScale.y, (m_ZCurve != null) ? m_ZCurve.Evaluate(1f) : localScale.z);
            if (!m_Target.gameObject.activeSelf)
            {
                m_Target.gameObject.SetActive(value: true);
            }
            m_Target = null;
            return true;
        }
        m_Target.localScale = new Vector3((m_XCurve != null) ? m_XCurve.Evaluate(num) * m_ValueCurveX : localScale.x, (m_YCurve != null) ? m_YCurve.Evaluate(num) * m_ValueCurveY : localScale.y, (m_ZCurve != null) ? m_ZCurve.Evaluate(num) : localScale.z);
        if (!m_Target.gameObject.activeSelf)
        {
            m_Target.gameObject.SetActive(value: true);
        }
        return false;
    }
}
