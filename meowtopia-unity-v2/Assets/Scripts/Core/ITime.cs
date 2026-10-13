using UnityEngine;
public interface ITime { float DeltaTime { get; } }
public sealed class UnityTime : ITime { public float DeltaTime => UnityEngine.Time.deltaTime; }