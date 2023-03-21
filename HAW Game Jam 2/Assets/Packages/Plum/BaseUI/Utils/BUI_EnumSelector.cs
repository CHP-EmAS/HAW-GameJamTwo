using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using TMPro;

namespace BaseUI.Utils{
    public class BUI_EnumSelector<T> : MonoBehaviour where T: System.Enum
    {
        [SerializeField] public T defaultValue;
        [SerializeField] private TMP_Dropdown dropdown;
        [SerializeField] private bool useForcedLowerCase;
        [SerializeField] private string[] shownStrings;
        public Utility.GenericDelegate<T> onValueChanged;
        protected void Load(bool useDefaultValue = true){
            if(dropdown == null) dropdown = GetComponent<TMP_Dropdown>();
            int length = GenericUtility<T>.GetEnumLength();
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < length; i++)
            {
                T type = (T)(object)i;  //https://stackoverflow.com/questions/10387095/cast-int-to-generic-enum-in-c-sharp
                TMP_Dropdown.OptionData optDat = new TMP_Dropdown.OptionData();

                string text = i < shownStrings.Length? shownStrings[i] : type.ToString();
                if(useForcedLowerCase){
                    text = text.Noun();
                }
                optDat.text = text;
                optionDatas.Add(optDat);
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(optionDatas);
            if(useDefaultValue) dropdown.value = (int)(object)defaultValue;
            dropdown.onValueChanged.AddListener(OnSelect);
        }

        public void SetValue(T type){
            dropdown.value = (int)(object)type;
            OnSelect((int)(object)type);
        }

        public virtual void OnSelect(int i){
            T type = (T)(object)i;
            onValueChanged?.Invoke(type);
        }
    }
}
