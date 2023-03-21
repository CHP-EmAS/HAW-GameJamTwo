using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    [System.Serializable]
    public struct Strct_Stringkeys{
        public List<string> content;
        public void Add(string item){
            if(content == null) content = new List<string>();
            if(content.Contains(item)) return;          //<- we dont want to save the same key twice!
            content.Add(item);
        }
        public Strct_Stringkeys(string[] cntnt){
            content = new List<string>();
            content.AddRange(cntnt);
        }
    }

    [CreateAssetMenu(fileName = "Strings", menuName = "Plum/Data/StringCollection", order = 0)]
    public class SCROB_StringArray : SCROBType<Strct_Stringkeys>
    {

    }
}


