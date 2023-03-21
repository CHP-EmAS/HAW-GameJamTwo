using System;
using UnityEngine;

namespace Music.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject m_projectile;

        private bool shot = false;

        private void Awake() 
        {
           Metronome.SubscribeOnMethod(0, OnNote);
        }

        private void Update()
        {
            shot = Input.GetKey(KeyCode.Mouse0);
        }

        private void OnNote()
        {
            if(shot)
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            
        }
    }
}