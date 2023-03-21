using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BaseUI{
    //https://answers.unity.com/questions/1166426/how-do-i-run-a-script-when-a-button-is-highlighted.html
    public abstract class BUI_FX_Button : Button
    {
        public Utility.ArgumentelessDelegate onEnter, onDown, onUp, onExit;
        protected override void Start()
        {
            base.Start();
            onClick.AddListener(OnClick);
        }
        public virtual void OnClick(){}
        public virtual void OnRelease(){}
        public virtual void OnSelect(){}
        public virtual void OnExit(){}
        public virtual void OnHighlighted(){}

#region Controller
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            OnHighlighted();
            onEnter?.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            OnExit();
            onExit?.Invoke();
        }

#endregion

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            OnHighlighted();
            onEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            OnExit();
            onExit?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnRelease();
            onDown?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnSelect();
            onUp?.Invoke();
        }

    }

}
