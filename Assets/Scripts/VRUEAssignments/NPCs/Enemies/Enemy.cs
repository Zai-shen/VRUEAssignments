using System;
using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

namespace VRUEAssignments.NPCs.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private SplineComputer _splineToFollow;
        private SplineFollower _splineFollower;
        private Animator _animator;

        [SerializeField]
        private int _healthPoints = 5;

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
        }

        private void ChangeSkin(EnemyKind kind)
        {
            Utils.Utils.SetAllChildren(gameObject, false);

            GameObject temp;
            switch (kind)
            {
                case EnemyKind.DRAGON:
                    temp = transform.Find("DragonMesh").gameObject;
                    break;
                case EnemyKind.GOLEM:
                    temp = transform.Find("GolemMesh").gameObject;
                    break;
                case EnemyKind.SKELETON:
                    temp = transform.Find("SkeletonMesh").gameObject;
                    break;
                case EnemyKind.SPIDER:
                    temp = transform.Find("SpiderMesh").gameObject;
                    break;
                case EnemyKind.TURTLE:
                    temp = transform.Find("TurtleShellMesh").gameObject;
                    break;
                case EnemyKind.MAGE:
                    temp = transform.Find("EvilMageMesh").gameObject;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            temp.SetActive(true);
            _animator = temp.GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("ClearArea"))
            {
                Die();
            }
        }

        public void OnHitByParticle(int damage)
        {
            _healthPoints -= damage;
            if (_healthPoints <= 0)
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