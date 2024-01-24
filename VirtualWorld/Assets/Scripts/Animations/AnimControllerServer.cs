using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Server;

namespace Animations
{
    // For now used for starting NPC idle animation
    public class AnimControllerServer : NetworkBehaviour
    {
        [SerializeField] ServerTick restartIdleAnimTick;

        private Animator _animator;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

        public override void OnStartServer()
        {
            base.OnStartServer();
            _animator = GetComponent<Animator>();
            AssignAnimationIDs();
            SetIdle();

            restartIdleAnimTick.OnTick.AddListener(OnServerTick);
        }

        private void OnServerTick()
        {
            Debug.Log("@@@ restarting animation... @@@");
            SetIdle();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void SetIdle()
        {
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetFloat(_animIDSpeed, 0);
            _animator.SetFloat(_animIDMotionSpeed, 1);
        }
    }
}
