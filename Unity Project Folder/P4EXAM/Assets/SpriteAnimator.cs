using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] animationFrames;
    private float frameRate = 10f; // frames per second

        [SerializeField]private float frameRateMAX = 100f;

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
            float adjustedFrameRate = Mathf.Clamp(frameRate * (1f + factoryBase.speedIncreasePercentage / 100f),0,frameRateMAX);
            int frameIndex = (int)(Time.time * adjustedFrameRate) % animationFrames.Length;

            //Debug.Log(adjustedFrameRate);
            spriteRenderer.sprite = animationFrames[frameIndex];

        }
    }
}

