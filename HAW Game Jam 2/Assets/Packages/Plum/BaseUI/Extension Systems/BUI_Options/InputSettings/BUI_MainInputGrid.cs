using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.IO;
using System.Text;
using BaseUI.Grid;

namespace BaseUI{
    public class BUI_MainInputGrid : MonoBehaviour
    {
        [System.Serializable]
        public struct BUI_Strct_InputBinding
        {
            public InputActionReference control;
            public int index;
        }

        private const string configKey = "KeyConfig";
        private const char keyWall = ';';

        [SerializeField] private BUI_Page parentPage;
        [SerializeField] private InputActionAsset control;
        [SerializeField, Header("optional automatiztion")] private BUI_Strct_InputBinding[] autoBindings;
        [SerializeField] private BUI_MainInputEntry prefabReference;
        private static InputActionRebindingExtensions.RebindingOperation rebinding;
        private Dictionary<string, string> keyConfig = new Dictionary<string, string>();    // INPUTACTIONNAME | KEYBINDING
        private bool isRebinding = false;
        private void Awake(){
            //v we want auto config if applied non manual bindings
            if(autoBindings.Length > 0)
            {
                BUI_MainInputEntry[] entries = GetComponentsInChildren<BUI_MainInputEntry>();
                foreach (var autoBinding in autoBindings)
                {
                    bool wasAssigned = false;
                    //v loop through all entires to check if this control was already assigned
                    foreach (var manualEntry in entries)
                    {
                        if (manualEntry.IsEqual(autoBinding))
                        {
                            wasAssigned = true;
                            break;
                        }
                    }

                    if (wasAssigned) continue;
                    else
                    {
                        //v instantiate new binding
                        BUI_MainInputEntry newEntry = Instantiate(prefabReference.gameObject, transform.position, Quaternion.identity, prefabReference.transform.parent).GetComponent< BUI_MainInputEntry>();
                        newEntry.AutoInit(autoBinding);
                    }
                }
            }
            LoadConfig();
        }

        private void OnDestroy(){
            SaveConfig();
        }

        private bool RebindUpdate(float n, TMPro.TextMeshProUGUI text){
            return isRebinding;
        }

        public void Rebind(InputActionReference toRebind, int index, TMPro.TextMeshProUGUI text){
            toRebind.action.Disable();
            rebinding = toRebind.action.PerformInteractiveRebinding(index).WithControlsExcluding("Mouse").OnComplete(operation => FinishRebind(toRebind, index, text)).Start();

            BUI_Strct_PopupConfig config = new BUI_Strct_PopupConfig();
            config.message = "press any key!";
            config.onUpdate = RebindUpdate;
            BUI_InvasivePopup.SpawnPopup(config, gameObject, parentPage);
            isRebinding = true;
        }

        private void FinishRebind(InputActionReference input, int index, TMPro.TextMeshProUGUI ttext){
            string text = InputControlPath.ToHumanReadableString(input.action.bindings[index].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
            ttext.text = text;

            //v key & binding
            string inputRef =  input.action.actionMap.name + "/"+ input.action.name + keyWall + index;
            string bindingRef = input.action.bindings[index].overridePath;

            if(keyConfig.ContainsKey(inputRef)){
                keyConfig[inputRef] = bindingRef;
            }else{
                keyConfig.Add(inputRef, bindingRef);
            }

            input.action.Enable();
            rebinding.Dispose();
            isRebinding = false;
        }

        private void LoadConfig(){
            control.Enable();
            SavingSystem.ReadBinaryStringDictionary(ref keyConfig, configKey);

            foreach (var item in keyConfig)
            {
                try{
                    string target = item.Key;
                    string actionKey = target;
                    string index = "";
                    for (int i = target.Length-1; i >= 0; i--)
                    {
                        //v slice save string to gain name & index
                        if(target[i] != keyWall){
                            index += target[i];
                            actionKey = actionKey.Remove(i);
                        }
                        else{
                            actionKey = actionKey.Remove(i);
                            break;
                        }
                    }

                    int iIndex = int.Parse(index.Reverse());

                    InputAction action = control.FindAction(actionKey);
                    action.ApplyBindingOverride(iIndex, new InputBinding{overridePath = item.Value});
                    action.Enable();
                } catch{
                    Debug.Log("config not found: " + item.Key);
                }
            }
        }

        private void SaveConfig(){
            SavingSystem.SaveBinaryStringDictionary(keyConfig, configKey);
        }

      
    }

}
