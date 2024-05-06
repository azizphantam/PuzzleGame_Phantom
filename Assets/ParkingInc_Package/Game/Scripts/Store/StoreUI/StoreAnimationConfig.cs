#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

[CreateAssetMenu(fileName ="Store Animation Config")]
public class StoreAnimationConfig : ScriptableObject
{
    [Header("Circle Animation")]
    [SerializeField] List<AnimationStageScale> circleScale; 
    [SerializeField] List<AnimationStageColor> circleColor;

    [Space]
    [SerializeField] List<AnimationStageScale> circleScaleBack;
    [SerializeField] List<AnimationStageColor> circleColorBack;

    public List<AnimationStageScale> CircleScale => circleScale;
    public List<AnimationStageColor> CircleColor => circleColor;

    public List<AnimationStageScale> CircleScaleBack => circleScaleBack;
    public List<AnimationStageColor> CircleColorBack => circleColorBack;

    [Header("Panel Animation")]
    [SerializeField] List<AnimationStageAnchoredPosition> panelPosition;
    [SerializeField] List<AnimationStageFade> panelFade;

    [Space]
    [SerializeField] List<AnimationStageAnchoredPosition> panelPositionBack;
    [SerializeField] List<AnimationStageFade> panelFadeBack;

    public List<AnimationStageAnchoredPosition> PanelPosition => panelPosition;
    public List<AnimationStageFade> PanelFade => panelFade;

    public List<AnimationStageAnchoredPosition> PanelPositionBack => panelPositionBack;
    public List<AnimationStageFade> PanelFadeBack => panelFadeBack;

    [Header("Preview Animation")]
    [SerializeField] List<AnimationStageAnchoredPosition> previewPosition;
    [SerializeField] List<AnimationStageFade> previewFade;

    [Space]
    [SerializeField] List<AnimationStageAnchoredPosition> previewPositionBack;
    [SerializeField] List<AnimationStageFade> previewFadeBack;

    public List<AnimationStageAnchoredPosition> PreviewPosition => previewPosition;
    public List<AnimationStageFade> PreviewFade => previewFade;

    public List<AnimationStageAnchoredPosition> PreviewPositionBack => previewPositionBack;
    public List<AnimationStageFade> PreviewFadeBack => previewFadeBack;


    [SerializeField, HideInInspector] float backDuration;
    public float BackDuration => backDuration;

    void OnValidate()
    {
        backDuration = Mathf.Max(
            GetListDuration(new List<AnimationStage>(CircleColorBack.ToArray())),
            GetListDuration(new List<AnimationStage>(circleScaleBack.ToArray())),


            GetListDuration(new List<AnimationStage>(PanelPositionBack.ToArray())),
            GetListDuration(new List<AnimationStage>(PanelFadeBack.ToArray())),

            GetListDuration(new List<AnimationStage>(PreviewPositionBack.ToArray())),
            GetListDuration(new List<AnimationStage>(PreviewFadeBack.ToArray()))
            );
    }


    private float GetListDuration(List<AnimationStage> stages)
    {
        float sum = 0;

        for(int i = 0; i < stages.Count; i++)
        {
            sum += stages[i].Delay;
            sum += stages[i].Duration;
        }

        return sum;
    }

    public abstract class AnimationStage
    {
        [SerializeField] float delay = 0;
        [SerializeField] float duration = 0.5f;
        [SerializeField] Ease.Type ease;

        public float Delay => delay;
        public float Duration => duration;
        public Ease.Type Ease => ease;
    }

    [System.Serializable]
    public class AnimationStageColor: AnimationStage
    {
        [SerializeField] Color minValue = Color.white;
        [SerializeField] Color maxValue = Color.black;

        public Color MinValue => minValue;
        public Color MaxValue => maxValue;
    }

    [System.Serializable]
    public class AnimationStageAnchoredPosition : AnimationStage
    {
        [SerializeField] Vector2 minValue;
        [SerializeField] Vector2 maxValue;
        public Vector2 MinValue => minValue;
        public Vector2 MaxValue => maxValue;
    }

    [System.Serializable]
    public class AnimationStageScale : AnimationStage
    {
        [SerializeField] Vector3 minValue;
        [SerializeField] Vector3 maxValue;
        public Vector3 MinValue => minValue;
        public Vector3 MaxValue => maxValue;
    }

    [System.Serializable]
    public class AnimationStageFade : AnimationStage
    {
        [SerializeField] float minValue;
        [SerializeField] float maxValue;
        public float MinValue => minValue;
        public float MaxValue => maxValue;
    }
}
