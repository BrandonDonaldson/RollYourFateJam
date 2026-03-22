using UnityEngine;

public class AnimatedObject : MonoBehaviour
{

    public SpriteRenderer render;
    public AnimationObject[] animations;

    [Header("View Only")]
    public int currentAnimationIndex = -1;
    public int currentFrame = 0;
    public float frameTimer = 0;
    public float timePerFrame = 0.5f;

    public void PlayAnim(int index)
    {
        if (currentAnimationIndex == index)
        {
            return;
        }
        currentAnimationIndex = index;
        timePerFrame = animations[currentAnimationIndex].timeBetweenFrames;
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
                    if (animations[currentAnimationIndex].looping)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = animations[currentAnimationIndex].animFrames.Length - 1;
                    }
                }

                render.sprite = animations[currentAnimationIndex].animFrames[currentFrame];
            }
        }
    }

}
