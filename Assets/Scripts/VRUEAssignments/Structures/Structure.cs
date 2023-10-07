using System;
using UnityEngine;

namespace Unity.Template.VR.VRUEAssignments.Structures
{
    [RequireComponent(typeof(Rigidbody),typeof(Renderer))]
    public class Structure : MonoBehaviour
    {
        [SerializeField] private float JointBreakForce = 14f;
        [SerializeField] private float JointBreakTorque = 3f;
        
        protected StructureInteractionType InteractionType = StructureInteractionType.Default;
        protected Renderer _meshRend;
        protected Rigidbody _rb;
        protected Collider _coll;
        protected FixedJoint _fj;
        
        private const string NAME_SUFFIX = " - Default";
        private const string TAG = "Structure";

        [Tooltip("Mass of a bowling pin")]private const float _rigidBodyMass = 1.3f;
        
        protected virtual void Awake()
        {
            SetIdentifiers();
            _meshRend = GetComponent<Renderer>();
            SetVisualMaterial();
            _rb = GetComponent<Rigidbody>();
            SetRigidbody();
            _coll = GetComponent<Collider>();
            SetPhysicalMaterial();
            _fj = gameObject.AddComponent<FixedJoint>();
            SetFixedJoint();
        }
        
        protected virtual void SetIdentifiers()
        {
            transform.name += NAME_SUFFIX;
            transform.tag = TAG;
        }
        
        protected virtual void SetRigidbody()
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rb.mass = _rigidBodyMass;
        }
        
        protected void SetFixedJoint()
        {
            // _fj.connectedBody = GameObject.FindGameObjectWithTag("Ground").GetComponent<Rigidbody>();
            _fj.breakForce = JointBreakForce;
            _fj.breakTorque = JointBreakTorque;
            _fj.enablePreprocessing = false;
            _fj.enableCollision = true;
        }
        
        protected virtual void SetVisualMaterial()
        {
            _meshRend.material = Resources.Load<Material>("Materials/Visual/asphalt_02");
        }

        protected virtual void SetPhysicalMaterial()
        {
            _coll.material = Resources.Load<PhysicMaterial>("Materials/Physical/StructureDefault");
        }

        protected void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        protected virtual void HandleCollision(Collision coll)
        {
            if (coll.transform.CompareTag("Throwable"))
            {
                Debug.Log($"New default collision with: {coll.transform.tag}");
            }
        }
    }
}