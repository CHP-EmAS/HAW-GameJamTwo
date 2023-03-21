using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.AI
{
    [System.Serializable]
    public struct AIBrainDesc{
    }
    
    [CreateAssetMenu(fileName = "Brain", menuName = "AI/BrainDesc", order = 0)]
    public class AI_BrainSCROB : ScriptableObject
    {
        public AIBrainDesc desc;
        public LayerMask viewLayers, groundLayers;
    }

}
