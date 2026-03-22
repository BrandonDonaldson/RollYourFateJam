using UnityEngine;

[CreateAssetMenu(fileName = "newAnimation", menuName = "New Animation Object")]
public class AnimationObject : ScriptableObject
{
    public Sprite[] animFrames;
    public float timeBetweenFrames = 0.25f;
    public bool looping = true;
}
