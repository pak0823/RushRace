using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundData")]
public class SoundData : ScriptableObject
{
    public string id;
    public eSOUNDTYPE type;
    public AudioClip clip;
    public float volume = 1f;
}
