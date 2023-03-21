using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BaseUI
{
    //v change your graphics-quality!
    public class BUI_GraphicsQuality : MonoBehaviour
    {
        [SerializeField] private BUI_Page associatedPage;
        [SerializeField] private TMP_Dropdown dropdown;
        private void Start(){
            //v subscribing the delegate
            associatedPage.onClose += OnPageEnd;
            LoadQualityLevels();
            dropdown.value = BUI_PlayerSettings.Instance.GetSettings().qualityIndex;

            dropdown.onValueChanged.AddListener(Event);
        }
        
        private void LoadQualityLevels(){
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                string name = QualitySettings.names[i];
                TMP_Dropdown.OptionData optDat = new TMP_Dropdown.OptionData();
                optDat.text = name;
                optionDatas.Add(optDat);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(optionDatas);

        }

        //v updating volume
        public void Event(int val)
        {
            BUI_PlayerSettings.Instance.UpdateQualitySettings(val);
        }

        //v saving on end
        private void OnPageEnd(){
            BUI_PlayerSettings.Instance.SaveSettings();
        }
    }

}
