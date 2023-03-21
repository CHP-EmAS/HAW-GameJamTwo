using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public interface IMoveable
    {
        public void Move(Vector3 direction);
        public void AddForce(Vector3 dir);
    }
}