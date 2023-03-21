using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Base
{
    public class Parenteer : MonoBehaviour
    {
        private class Parented
        {
            public GameObject held;
            public Transform initialParent;
            public Parented(GameObject held)
            {
                this.held = held;
                initialParent = held.transform.parent;
            }
        }
        [SerializeField] private List<string> excludeTags = new List<string>();
        private Dictionary<GameObject, Parented> linker = new Dictionary<GameObject, Parented>();

        private void Parent(GameObject target)
        {
            if (excludeTags.Contains(target.tag))
            {
                return;
            }


            Parented p = new Parented(target);
            linker.Add(gameObject, p);
            target.transform.parent = transform;
        }

        private void UnParent(GameObject target)
        {
            if (!linker.ContainsKey(target)) return;
            Parented p = linker[target];
            target.transform.parent = p.initialParent;
            linker.Remove(target);
        }

#region REGISTER_T

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject g = collision.gameObject;
            Parent(g);
        }

        private void OnTriggerEnter(Collider collision)
        {
            GameObject g = collision.gameObject;
            Parent(g);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameObject g = collision.gameObject;
            UnParent(g);
        }

        private void OnTriggerExit(Collider collision)
        {
            GameObject g = collision.gameObject;
            UnParent(g);
        }
#endregion
    }
}
