using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] animationFrames;
    public float frameRate = 10f; // frames per second

    private FactoryBase factoryBase;

    void Awake()
    {
        factoryBase = GetComponent<FactoryBase>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        if (animationFrames.Length == 0 || factoryBase == null) return;

        if (factoryBase.state == FactoryState.Crafting)
        {
            float adjustedFrameRate = frameRate * (1f + factoryBase.speedIncreasePercentage / 100f);
            int frameIndex = (int)(Time.time * adjustedFrameRate) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[frameIndex];

        }
    }
}

