using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BaseUI{
    public class BUI_MainInputEntry : MonoBehaviour
    {
        [SerializeField, Header("config")] private InputActionReference targetInput;
        [SerializeField] private int rebindControlIndex = 0;
        [SerializeField, Header("references")] private BUI_MainInputGrid parent;    
        [SerializeField] private TMPro.TextMeshProUGUI keyText, bindText;
        //v this HAS to be start :D
        private void Start(){
            keyText.text = targetInput.action.name;
            bindText.text = InputControlPath.ToHumanReadableString(targetInput.action.bindings[rebindControlIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        public void Rebind(){
            parent.Rebind(targetInput, rebindControlIndex, bindText);
        }

        public bool IsEqual(BUI_MainInputGrid.BUI_Strct_InputBinding binding)
        {
            return ReferenceEquals(binding.control, targetInput) && rebindControlIndex == binding.index;
        }

        public void AutoInit(BUI_MainInputGrid.BUI_Strct_InputBinding binding)
        {
            rebindControlIndex = binding.index;
            targetInput = binding.control;
            Start();
        }
    }
}
