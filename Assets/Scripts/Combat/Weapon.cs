using System;
using Music.Instrument;
using UnityEngine;
using UnityEngine.Pool;

namespace Music.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Projectile m_projectilePrefab;
        private ProjectileInitSettings _projectileInitSettings;
        private ObjectPool<Projectile> _projectilePool;
        [SerializeField] private bool m_projectilePoolCollectionCheck = false;
        [SerializeField] private int m_defaultProjectilePoolSize = 30;
        [SerializeField] private int m_maxProjectilePoolSize = 1000;
        [SerializeField] private Transform m_projectileSpawnTransform;
        
        private bool _shot = false;
        
        private void Start()
        {
            _projectileInitSettings = new ProjectileInitSettings();

            _projectileInitSettings.ReleaseAction = ReleaseProjectile;
            _projectileInitSettings.SpawnTransform = m_projectileSpawnTransform;
            _projectileInitSettings.Speed = 5;
            _projectileInitSettings.LifeSpan = 3;
            
            _projectilePool = new ObjectPool<Projectile>(
                () =>
                {
                    Projectile projectile = Instantiate(m_projectilePrefab);
                    projectile.Initialize(_projectileInitSettings);
                    return projectile;
                },
                projectile =>
                {
                    projectile.Initialize(_projectileInitSettings);
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
                SpawnProjectile();
            }
        }

        private void OnNote(object sender, Melanchall.DryWetMidi.Multimedia.NotesEventArgs args)
        {
            if(_shot)
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            Projectile projectile = _projectilePool.Get();
        }

        private void ReleaseProjectile(Projectile projectile)
        {
            _projectilePool.Release(projectile);
        }
    }
}