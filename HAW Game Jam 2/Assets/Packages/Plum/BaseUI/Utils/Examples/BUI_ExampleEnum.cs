using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Utils.Examples{
    
    public enum Enum_BUIExample{
        THIS,
        IS,
        AN,
        EXAMPLE
    }
    public class BUI_ExampleEnum : BUI_EnumSelector<Enum_BUIExample>
    {
        private void Start(){
            Load();
        }
    }
}
