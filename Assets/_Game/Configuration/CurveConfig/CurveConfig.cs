using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurveConfig", menuName = "Configuration/Curve/CurveConfig", order = 0)]
public class CurveConfig : ScriptableObject
{
    public List<CurveItemConfig> ListCurves = new List<CurveItemConfig> ();

    public AnimationCurve GetCurveByType(CurveType curveType) => this.ListCurves.Find(x => x.curveType == curveType).Curve;
}

