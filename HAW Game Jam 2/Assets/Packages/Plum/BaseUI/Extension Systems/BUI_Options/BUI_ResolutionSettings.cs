using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BaseUI
{
    public class BUI_ResolutionSettings : MonoBehaviour
    {
        [SerializeField] private BUI_Page associatedPage;
        [SerializeField] private TMP_Dropdown dropdown;
        private int lastIndex;
        private void Start()
        {
            //v subscribing the delegate
            associatedPage.onClose += OnPageEnd;
            LoadResolutions();
            dropdown.value = Screen.resolutions.Length - 1 - BUI_PlayerSettings.Instance.GetSettings().resolutionIndex;
            lastIndex = dropdown.value;

            dropdown.onValueChanged.AddListener(Event);
        }

        private string GenerateResoltionString(Resolution n)
        {
            string h = n.width + "x" + n.height + "|" + n.refreshRate + "hz";
            return h;
        }

        private void LoadResolutions()
        {
            List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
            int length = Screen.resolutions.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                Resolution r = Screen.resolutions[i];
                string name = GenerateResoltionString(r);
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
            BUI_PlayerSettings.Instance.UpdateResolutionSettings(val);

            BUI_Strct_PopupConfig config = new BUI_Strct_PopupConfig();
            config.maxTimer = maxPopupTime;
            config.message = "Keep Resolution?";
            config.onDiscard = ClosePopupN;
            config.onSuccess = ClosePopupY;
            config.onUpdate = MsgUpdt;

            BUI_InvasivePopup.SpawnPopup(config, gameObject, associatedPage);
        }

        #region Popup
        private const float maxPopupTime = 10;
        private void ClosePopupY(){
            //apply new index
            lastIndex = dropdown.value;
        }

        private void ClosePopupN(){
            //v closing popup without triggering it again 
            dropdown.SetValueWithoutNotify(lastIndex);
            BUI_PlayerSettings.Instance.UpdateResolutionSettings(lastIndex);
        }

        private bool MsgUpdt(float timer, TMPro.TextMeshProUGUI text){
            text.text = "Keep Resolution? " + (int)(maxPopupTime - timer);
            return true;
        }

        #endregion

        //v saving on end
        private void OnPageEnd()
        {
            BUI_PlayerSettings.Instance.SaveSettings();
        }
    }
}

