using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Music.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject m_projectile;

        private ObjectPool<Projectile> _projectilePool;

        private bool shot = false;

        private void Awake() 
        {
           Metronome.SubscribeOnMethod(0, OnNote);
        }

        private void Start()
        {
           _projectilePool = new ObjectPool<Projectile>()
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