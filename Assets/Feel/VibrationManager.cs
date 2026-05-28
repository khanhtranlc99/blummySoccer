using Lofelt.NiceVibrations;
using System;
using System.Collections.Generic;

public class VibrationManager : MonoSingleton<VibrationManager>
{
    public List<VibrationData> ListDatas = new List<VibrationData>();
    public void PlayVibrationByType(VIBRATION_TYPE _type)
    {
        switch (_type)
        {
            case VIBRATION_TYPE.SELECTION:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
                break;
            case VIBRATION_TYPE.SUCCESS:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                break;
            case VIBRATION_TYPE.WARNING:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
                break;
            case VIBRATION_TYPE.FAILURE:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
                break;
            case VIBRATION_TYPE.RIGID:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
                break;
            case VIBRATION_TYPE.SOFT:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
                break;
            case VIBRATION_TYPE.LIGHT:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                break;
            case VIBRATION_TYPE.MEDIUM:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                break;
            case VIBRATION_TYPE.HEAVY:
                HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                break;

            default:
                VibrationData data = this.ListDatas.Find(x => x._type == _type);
                if (data == null)
                    data = this.ListDatas[0];
                HapticController.Play(data.Clip);
                break;
        }
    }
}
public enum VIBRATION_TYPE
{
    CARILLON = 1,
    DICE = 2,
    DRUMSLOOP = 3,
    GAME_OVER = 4,
    HEART_BEAT = 5,
    LASER = 6,
    POWER_OFF = 7,
    RELOAD = 8,
    TELEPORT = 9,

    SELECTION = 10,
    SUCCESS = 11,
    WARNING = 12,
    FAILURE = 13,
    RIGID = 14,
    SOFT = 15,
    LIGHT = 16,
    MEDIUM = 17,
    HEAVY = 18
}
[Serializable]
public class VibrationData
{
    public VIBRATION_TYPE _type;
    public HapticClip Clip;
}
