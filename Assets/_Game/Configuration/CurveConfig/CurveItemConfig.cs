using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CurveItemConfig", menuName = "Configuration/Curve/CurveItemConfig", order = 0)]
public class CurveItemConfig : ScriptableObject
{
    public CurveType curveType;
    public AnimationCurve Curve;
}
[Serializable]
public enum CurveType
{
    SOFT_BOUNCE = 0,
    LIGHT_BOUNCE = 1,
    MEDIUM_BOUNCE = 2,
    HEAVY_BOUNCE = 3
}
[Serializable]
public class CurveData
{
    public CurveType curveType;
    public AnimationCurve curve;
}

