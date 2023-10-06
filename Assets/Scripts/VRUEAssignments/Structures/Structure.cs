using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Unity.Template.VR.VRUEAssignments.Structures
{
    [RequireComponent(typeof(Rigidbody),typeof(Renderer))]
    public class Structure : MonoBehaviour
    {
        protected StructureInteractionType InteractionType = StructureInteractionType.Default;
        protected Renderer _meshRend;
        protected Rigidbody _rb;
        protected Collider _coll;
        protected FixedJoint _fj;
        
        protected virtual void Awake()
        {
            SetName();
            _meshRend = GetComponent<Renderer>();
            SetVisualMaterial();
            _rb = GetComponent<Rigidbody>();
            SetRigidbody();
            _coll = GetComponent<Collider>();
            SetPhysicalMaterial();
            // _fj = gameObject.AddComponent<FixedJoint>();
            // SetFixedJoint();
        }
        
        protected virtual void SetName()
        {
            transform.name += " - Default";
        }
        
        protected virtual void SetRigidbody()
        {
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        
        protected void SetFixedJoint()
        {
            // _fj.connectedBody = GameObject.FindGameObjectWithTag("Ground").GetComponent<Rigidbody>();
            // _fj.enablePreprocessing = false;
            // _fj.breakForce = 1000f;
            // _fj.breakTorque = 100f;
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
            Debug.Log($"New collision with: {coll.transform.tag}");
        }
    }
}