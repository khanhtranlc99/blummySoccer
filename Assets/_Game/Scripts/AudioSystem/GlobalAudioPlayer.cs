using UnityEngine;

public static class GlobalAudioPlayer
{

    public static AudioPlayer audioPlayer;
    public static void ChangeVolumeMusic(float value)
    {
        if (audioPlayer != null)
            AudioPlayer.MusicSource.volume = AudioPlayer.MusicSource.volume == 0f ? 0f : value;
    }
    public static void PlaySFX(eAudioType audioType)
    {
        if (audioPlayer != null) audioPlayer.playSFX(audioType);
    }
    public static void PlaySFXWithAudioItem(AudioItem item)
    {
        if (audioPlayer != null && item != null) audioPlayer.playSFXWithAudioItem(item);
    }
    public static void PlayLoopSFX(eAudioType audioType, float time, Transform parent = null)
    {
        if (audioPlayer != null) audioPlayer.playLoopSFX(audioType, time, parent);
    }
    public static void PlayMultipleSFX(eAudioType audioType)
    {
        if (audioPlayer != null) audioPlayer.playMultipleSFX(audioType);
    }
    public static void PlaySFXAtPosition(eAudioType audioType, Vector3 position)
    {
        if (audioPlayer != null) audioPlayer.playSFXAtPosition(audioType, position);
    }

    public static void PlaySFXAtPosition(eAudioType audioType, Vector3 position, Transform parent)
    {
        if (audioPlayer != null) audioPlayer.playSFXAtPosition(audioType, position, parent);
    }
    public static void PlayeMusicFromIntro(eAudioType audioType)
    {
        if (audioPlayer != null) audioPlayer.PlayMusicFromIntro(audioType);
    }
    public static void PlayMusic(eAudioType audioType)
    {
        if (audioPlayer != null) audioPlayer.playMusic(audioType);
    }
}