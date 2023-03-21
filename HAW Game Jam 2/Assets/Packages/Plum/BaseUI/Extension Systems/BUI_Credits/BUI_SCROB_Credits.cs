using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUI.Credits{    
    [System.Serializable]
    public struct CreditEntry{
        public string title;
        public string second;
        public string third;
    }

    [System.Serializable]
    public struct CreditCategory{
        public string title;
        public CreditEntry[] entries;
    }

    [CreateAssetMenu(fileName = "Credits", menuName = "BUI/Credits", order = 0)]
    public class BUI_SCROB_Credits : ScriptableObject
    {
        public string left = "LEFT", middle = "MIDDLE", right = "RIGHT";
        public CreditCategory[] credits;
    }
}
