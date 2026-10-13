using UnityEngine;

[CreateAssetMenu(menuName = "Pet/Need")]
public class PetNeed : ScriptableObject {
    public string needId;
    public float value = 100f;          // 0..100
    public float decayPerSecond = 1f;   // how fast it drops
    public float lowThreshold = 25f;    // mood penalties below this
    public float highThreshold = 75f;   // mood bonuses above this
}