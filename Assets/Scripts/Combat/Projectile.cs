using System;
using UnityEngine;

namespace Music.Combat
{
    public class Projectile : MonoBehaviour
    {
        public static ProjectileDelegate OnCollision;
        
        public delegate void ProjectileDelegate(Projectile instance);
        public void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(this);
        }

        public void Explode()
        {
            
        }
    }
}