using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace BaseUI{
    public class BUI_FX_Scale : BUI_FX_Button
    {
        [SerializeField] private bool automateColors;
        private const float wobbleSpeed = 15, wobbleIntensity = 25, downScaleFactor = 1.08f;
        private Vector3 originalScale, targetScale;
        private bool queuedRawSet = false;
        public Utility.ArgumentelessDelegate onSelected, onReleased, onHighlighted;
        protected override void Start(){
            base.Start();
            transform.localScale = Vector3.one;
            originalScale = transform.localScale;
            targetScale = originalScale;

            if(automateColors){
                RecalculateColor();
            }
        }

        public void RecalculateColor(){
            Color baseColor = targetGraphic.color;
            targetGraphic.color = Color.white;
            ColorBlock newColors = colors;

            newColors.normalColor = baseColor;

            newColors.highlightedColor = baseColor;
            newColors.highlightedColor.ChangeSaturation(-.1f);
            newColors.highlightedColor.ChangeValue(.1f);

            newColors.pressedColor = baseColor;
            newColors.pressedColor.ChangeValue(-.1f);
            newColors.pressedColor.ChangeSaturation(.1f);

            newColors.selectedColor = baseColor;
            newColors.highlightedColor.ChangeSaturation(-.05f);
            newColors.highlightedColor.ChangeValue(.05f);

            newColors.disabledColor = baseColor;
            newColors.disabledColor = new Color(baseColor.r, baseColor.g, baseColor.b, .25f);

            colors = newColors;
        }

        public void RecalculateColor(Color target){
            Color baseColor = target;
            targetGraphic.color = Color.white;
            ColorBlock newColors = colors;

            newColors.normalColor = baseColor;

            newColors.highlightedColor = baseColor;
            newColors.highlightedColor.ChangeSaturation(-.1f);
            newColors.highlightedColor.ChangeValue(.1f);

            newColors.pressedColor = baseColor;
            newColors.pressedColor.ChangeValue(-.1f);
            newColors.pressedColor.ChangeSaturation(.1f);

            newColors.selectedColor = baseColor;
            newColors.highlightedColor.ChangeSaturation(-.05f);
            newColors.highlightedColor.ChangeValue(.05f);

            newColors.disabledColor = baseColor;
            newColors.disabledColor = new Color(baseColor.r, baseColor.g, baseColor.b, .25f);

            colors = newColors;
        }

        private Vector3 queued;
        public void SetScale(Vector3 target){
            queued = target;
            queuedRawSet = true;
        }

        private void Update(){
            if(queuedRawSet){
                transform.localScale = queued;
                queuedRawSet = false;
            }
            transform.localScale = Utility.WobbleLerpV3(transform.localScale, targetScale, ref refv0, wobbleSpeed, wobbleIntensity, Time.unscaledDeltaTime);
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 0, originalScale.x * 2),
            Mathf.Clamp(transform.localScale.y, 0, originalScale.y * 2),
            Mathf.Clamp(transform.localScale.z, 0, originalScale.z * 2));
        }
        private Vector3 refv0, refv1;

        public override void OnSelect()
        {
           targetScale = originalScale * downScaleFactor;
           onSelected?.Invoke();
        }

        public override void OnRelease()
        {
            targetScale = originalScale;
            onReleased?.Invoke();
        }

        public override void OnHighlighted()
        {
            float sFactor = .04f;
            transform.localScale += new Vector3(sFactor, sFactor, sFactor);
            onHighlighted?.Invoke();
        }
    }
}
