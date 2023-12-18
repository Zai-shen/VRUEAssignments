using System;
using System.Collections;
using Dreamteck.Splines;
using UnityEngine;
using VRUEAssignments.UI;

namespace VRUEAssignments.NPCs.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private SplineComputer _splineToFollow;
        private SplineFollower _splineFollower;
        private Animator _animator;

        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Transform _meshes;
        
        
        [SerializeField] private int _maxHealthPoints = 5;
        [SerializeField] private int _currentHealthPoints = 5;
        [SerializeField] private int _damage = 2;
        [SerializeField] private float _followSpeed = 0.2f;

        private void Awake()
        {
            _splineFollower = GetComponent<SplineFollower>();
        }

        public void SetSplineToFollow(SplineComputer splineComputer)
        {
            _splineToFollow = splineComputer;
            _splineFollower.spline = splineComputer;
            _splineFollower.follow = true;
        }

        public void ChangeEnemy(EnemyKind kind)
        {
            ChangeSkin(kind);
            ChangeStats(kind);
            ChangeSpeed(kind);
        }
        
        private void ChangeSpeed(EnemyKind kind)
        {
            switch (kind)
            {
                case EnemyKind.DRAGON:
                    ChangeSpeed(0.4f);
                    break;
                case EnemyKind.GOLEM:
                    ChangeSpeed(0.2f);
                    break;
                case EnemyKind.SKELETON:
                    ChangeSpeed(0.7f);
                    break;
                case EnemyKind.SPIDER:
                    ChangeSpeed(0.4f);
                    break;
                case EnemyKind.TURTLE:
                    ChangeSpeed(0.2f);
                    break;
                case EnemyKind.MAGE:
                    ChangeSpeed(0.6f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private void ChangeSpeed(float speed)
        {
            _followSpeed = speed;
            _splineFollower.followSpeed = speed;
        }
        
        private void ChangeStats(EnemyKind kind)
        {
            switch (kind)
            {
                case EnemyKind.DRAGON:
                    ChangeStats(2,1);
                    break;
                case EnemyKind.GOLEM:
                    ChangeStats(4,1);
                    break;
                case EnemyKind.SKELETON:
                    ChangeStats(2,2);
                    break;
                case EnemyKind.SPIDER:
                    ChangeStats(1,3);
                    break;
                case EnemyKind.TURTLE:
                    ChangeStats(3,1);
                    break;
                case EnemyKind.MAGE:
                    ChangeStats(4,3);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private void ChangeStats(int hp, int dmg)
        {
            _maxHealthPoints = hp;
            _currentHealthPoints = hp;
            _healthBar.UpdateHealthBar(_maxHealthPoints, _currentHealthPoints);
            _damage = dmg;
        }
        
        private void ChangeSkin(EnemyKind kind)
        {
            Utils.Utils.SetAllChildren(_meshes.gameObject, false);

            GameObject temp;
            switch (kind)
            {
                case EnemyKind.DRAGON:
                    temp = _meshes.Find("DragonMesh").gameObject;
                    break;
                case EnemyKind.GOLEM:
                    temp = _meshes.Find("GolemMesh").gameObject;
                    break;
                case EnemyKind.SKELETON:
                    temp = _meshes.Find("SkeletonMesh").gameObject;
                    break;
                case EnemyKind.SPIDER:
                    temp = _meshes.Find("SpiderMesh").gameObject;
                    break;
                case EnemyKind.TURTLE:
                    temp = _meshes.Find("TurtleShellMesh").gameObject;
                    break;
                case EnemyKind.MAGE:
                    temp = _meshes.Find("EvilMageMesh").gameObject;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            temp.SetActive(true);
            _animator = temp.GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Base"))
            {
                TileBase tBase;
                if (other.TryGetComponent<TileBase>(out tBase))
                {
                    tBase.TakeDamage(_damage);
                }
                if (other.transform.parent.TryGetComponent<TileBase>(out tBase))
                {
                    tBase.TakeDamage(_damage);
                }
                Die();
            }
        }

        public void OnHitByParticle(int damage)
        {
            TakeDamage(damage);
        }

        private void TakeDamage(int damage)
        {
            _currentHealthPoints -= damage;
            _healthBar.UpdateHealthBar(_maxHealthPoints, _currentHealthPoints);
            if (_currentHealthPoints <= 0)
            {
                DieAndStop();
            }
            else
            {
                _animator.SetTrigger("getHit");
            }
        }

        private void Die()
        {
            _animator.SetTrigger("getShot");
            StartCoroutine(BlinkAndDestroy());
        }

        private void DieAndStop()
        {
            Die();
            _splineFollower.follow = false;
        }

        private IEnumerator BlinkAndDestroy()
        {
            float duration = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(duration);
            Destroy(this.gameObject);
        }
    }
}