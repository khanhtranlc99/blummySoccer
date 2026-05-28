using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour
{
    private bool m_IsUserAuthen = false;
    private bool m_IsRateUs = false;
    protected int m_CurrentLevel;
    private bool m_IsSoundOn;
    private bool m_IsMusicOn;
    private bool m_IsVibrateOn;
    protected bool m_IsFirstOpenGame;
    public bool IsUserAuthen
    {
        get
        {
            return this.m_IsUserAuthen;
        }
        set
        {
            this.m_IsUserAuthen = value;
            PlayerPrefs.SetInt("IsUserAuthen", value ? 1 : 0);
        }
    }
    public bool IsRateUs
    {
        get
        {
            return this.m_IsRateUs;
        }
        set
        {
            this.m_IsRateUs = value;
            PlayerPrefs.SetInt("IsRateUs", value ? 1 : 0);
        }
    }
    public int CurrentLevel{
        get{
            return this.m_CurrentLevel;
        }
        set{
            this.m_CurrentLevel = value;
            PlayerPrefs.SetInt("CurrentLevel", value);
        }
    }
    public bool IsSoundOn
    {
        get
        {
            return this.m_IsSoundOn;
        }
        set
        {
            this.m_IsSoundOn = value;
            PlayerPrefs.SetInt("IsSoundOn", value ? 1 : 0);
        }
    }
    public bool IsMusicOn
    {
        get
        {
            return this.m_IsMusicOn;
        }
        set
        {
            this.m_IsMusicOn = value;
            PlayerPrefs.SetInt("IsMusicOn", value ? 1 : 0);
        }
    }
    public bool IsVibrateOn
    {
        get
        {
            return this.m_IsVibrateOn;
        }
        set
        {
            this.m_IsVibrateOn = value;
            PlayerPrefs.SetInt("IsVibrateOn", value ? 1 : 0);
        }
    }
    public bool IsFirstOpenGame
    {
        get{
            return this.m_IsFirstOpenGame;
        }
        set{
            this.m_IsFirstOpenGame = value;
            PlayerPrefs.SetInt("IsFirstOpenGame", (true) ? 1 : -1);
        }
    }
    protected virtual void Awake(){
        this.m_IsRateUs = PlayerPrefs.GetInt("IsRateUs", 0) == 1 ? true : false;
        m_CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        m_IsSoundOn = PlayerPrefs.GetInt("IsSoundOn", 1) == 1 ? true : false;
        m_IsMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1 ? true : false;
        m_IsVibrateOn = PlayerPrefs.GetInt("IsVibrateOn", 1) == 1 ? true : false;
        m_IsFirstOpenGame = (PlayerPrefs.GetInt("IsFirstOpenGame", 1) == 1) ? true : false;
        m_IsUserAuthen = PlayerPrefs.GetInt("IsUserAuthen", 0) == 1 ? true : false;
    }
    public string BundleVersionCode
    {
        get
        {
            return Application.version.ToString();
        }
    }
}
