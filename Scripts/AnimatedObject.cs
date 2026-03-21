using UnityEngine;

public class AnimatedObject : MonoBehaviour
{

    public SpriteRenderer render;
    public AnimationObject[] animations;
    public float timePerFrame = 0.5f;

    [Header("View Only")]
    public int currentAnimationIndex = -1;
    public int currentFrame = 0;
    public float frameTimer;

    public void PlayAnim(int index)
    {
        currentAnimationIndex = index;
        currentFrame = 0;
        frameTimer = 0;
    }

    private void Update()
    {
        frameTimer += Time.deltaTime;
        if (currentAnimationIndex != -1)
        {
            //Playing an anim
            if (frameTimer > timePerFrame)
            {
                frameTimer = 0;
                currentFrame++;
                if (currentFrame >= animations[currentAnimationIndex].animFrames.Length)
                {
                    currentFrame = 0;
                }

                render.sprite = animations[currentAnimationIndex].animFrames[currentFrame];
            }
        }
    }

}
