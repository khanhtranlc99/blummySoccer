using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    public float slowdownFactor = .05f;
    public float slowdownLength = 2f;
    public bool enableSlow = false;

    private void Update()
    {
        if (!enableSlow) return;
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, .35f);
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        this.enableSlow = true;
    }

    public void OnReset()
    {
        this.enableSlow = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
