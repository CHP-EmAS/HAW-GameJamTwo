using System;
using Music.Instrument;
using UnityEngine;
using UnityEngine.Pool;
using Plum.VFX;

namespace Music.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Projectile m_projectilePrefab;
        private ObjectPool<Projectile> _projectilePool;
        [SerializeField] private bool m_projectilePoolCollectionCheck = false;
        [SerializeField] private int m_defaultProjectilePoolSize = 30;
        [SerializeField] private int m_maxProjectilePoolSize = 1000;
        [SerializeField] private ProjectileInitSettings settings;
        
        private bool _shot = false;
        
        private void Start()
        {
            settings.ReleaseAction = ReleaseProjectile;
            
            _projectilePool = new ObjectPool<Projectile>(
                () =>
                {
                    Projectile projectile = Instantiate(m_projectilePrefab);
                    projectile.Initialize(settings);
                    return projectile;
                },
                projectile =>
                {
                    projectile.Initialize(settings);
                }, 
                projectile =>
                {
                    projectile.Release();
                }, 
                projectile =>
                {
                    Destroy(projectile.gameObject);
                }, 
                m_projectilePoolCollectionCheck, 
                m_defaultProjectilePoolSize, 
                m_maxProjectilePoolSize
            );
            
            Metronome.SubscribeOnMethod(InstrumentType.Kick, OnNote);
        }

        private void Update()
        {
            _shot = Input.GetKey(KeyCode.Mouse0);
            if(_shot)
            {
                //SpawnProjectile();
            }
        }

        private void OnNote(float input)
        {
            if(_shot)
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            Music.Player.MainCam.RequestShake(.5f, 1.0f);
            Projectile projectile = _projectilePool.Get();
        }

        private void ReleaseProjectile(Projectile projectile)
        {
            _projectilePool.Release(projectile);
        }
    }
}