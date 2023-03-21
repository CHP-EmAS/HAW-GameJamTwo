using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Plum.Base;
using TMPro;

namespace BaseUI
{       
    public struct BUI_Strct_PopupConfig{
        public string message;
        public float maxTimer;
        public System.Action onSuccess, onDiscard;
        public System.Func<float, TextMeshProUGUI, bool> onUpdate;
    }
    
    public class BUI_InvasivePopup : MonoBehaviour
    {
#region STATIC
        //v PBaseScripts already relies on addressables, so this shouldn't be a problem :3
        private const string prefabKey = "BUI_Popup";

        public static bool isActive = false;
        private static BUI_InvasivePopup instance;
        //v we can spawn when required and then also reuse
        public static BUI_InvasivePopup SpawnPopup(BUI_Strct_PopupConfig config, GameObject from, BUI_Page parent){
            BUI_InvasivePopup popup;
            if(instance == null){
                popup = AssetLoad<BUI_InvasivePopup>.Get(prefabKey);
            } else{
                popup = instance;
                popup.gameObject.SetActive(true);
            }
            popup.Configure(config, parent);
            return popup;
        }
#endregion

#region INSTANCED
        [SerializeField] private Button yes, no;
        [SerializeField] private TextMeshProUGUI text;
        private BUI_Strct_PopupConfig settings;
        private float timer = 0;
        private Selectable lastSelected;
        private void Awake(){
            InputHandler.Instance.AddWorkCondition(Cond);

            gameObject.transform.parent = BUI_MainCanvas.Instance.transform;
            gameObject.transform.SetAsLastSibling();
            gameObject.transform.localPosition = Vector3.zero;
        }

        private bool Cond(){return !isActive;}

        //v happens directly after initialization
        public void Configure(BUI_Strct_PopupConfig config, BUI_Page parent){
            lastSelected = parent.FirstSelectable;

            settings = config;

            text.text = config.message;

            if(config.onSuccess != null){
                yes.onClick.AddListener(delegate{config.onSuccess();});        
                yes.onClick.AddListener(delegate{Close();});        
                yes.gameObject.SetActive(true);
            } else{
                yes.gameObject.SetActive(false);
            }

            if(config.onDiscard != null){
                no.onClick.AddListener(delegate{config.onDiscard();});
                no.onClick.AddListener(delegate{Close();});
                no.gameObject.SetActive(true);
            } else{
                no.gameObject.SetActive(false);
            }


            if(settings.maxTimer <= 0 && config.onSuccess == null && config.onDiscard == null) Debug.Log("WARNING: initialized InvPopup without closure opportunity!");
            timer = 0;
            isActive = true;
            instance = this;
        }

        private void Update(){
            if(settings.onUpdate != null){
                if(!settings.onUpdate.Invoke(timer, text)) Close();
            }


            if(settings.maxTimer > 0){
                timer += Time.deltaTime;
                if(timer >= settings.maxTimer){
                    settings.onDiscard?.Invoke();
                    Close();
                }
            }else{
                //no timer required
            }
        }


        private void Close(){
            gameObject.SetActive(false);
            isActive = false;
            if(Plum.Base.Controls.HasJoyStickActive()) lastSelected.Select();
        }


        //v but also destroy when not necessary any more to avoid singletons
        private void OnDestroy(){
            if(instance == this) instance = null;
            InputHandler.Instance.RemoveWorkCondition(Cond);
        }
#endregion
    }

}
