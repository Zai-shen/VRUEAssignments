using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    private CharacterController _cController;
    private bool _isGrounded;
    private Vector3 _playerVelocity;
    private float _playerSpeed = 2.0f;
    private float _jumpHeight = 1.0f;
    private float _gravityValue = -9.81f;

    
    private void Start()
    {
        _cController = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Prevent control is connected to Photon and represent the localPlayer
        if( photonView.IsMine == false && PhotonNetwork.IsConnected == true )
        {
            return;
        }
        
        _isGrounded = _cController.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _cController.Move(move * Time.deltaTime * _playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _cController.Move(_playerVelocity * Time.deltaTime);
    }
}
