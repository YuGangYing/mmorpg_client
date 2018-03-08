﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	[RequireComponent(typeof(MMOUnitSkill))]
	public class MMOUnit : MonoBehaviour
	{
		public UnitInfo unitInfo;
		public float animationSpeedOffset = 3;
		Transform mTrans;
		SimpleRpgAnimator mSimpleRpgAnimator;
		Animator mAnimator;

		CapsuleCollider mCapsuleCollider;
		CharacterController mCharacterController;

		void Awake(){
			mTrans = transform;
			mSimpleRpgAnimator = GetComponent<SimpleRpgAnimator> ();
			mAnimator =  GetComponentInChildren<Animator> (true);
			mCapsuleCollider = GetComponent<CapsuleCollider> ();
			mCharacterController = GetComponent<CharacterController> ();
		}

		void Update(){
			#if NET_SERVER
			unitInfo.transform.playerPosition = mTrans.position;
			unitInfo.transform.playerForward = mTrans.forward;
			#endif
		}

		public Vector3 GetHeadPos(){
			if (mCharacterController != null)
				return mTrans.position + new Vector3 (0, mCharacterController.height, 0);
			else if (mCapsuleCollider != null)
				return mTrans.position + new Vector3 (0, mCapsuleCollider.height, 0);
			else
				return mTrans.position;
		}

		string mPreAction;
		public void SetAnimation(string action,float speed){
			if (mSimpleRpgAnimator != null) {
				mSimpleRpgAnimator.Action = action;
				mSimpleRpgAnimator.SetSpeed (speed * animationSpeedOffset);
			} else if (mAnimator !=null){
				//TODO
				if (mPreAction != action) {
					if (mPreAction == "walk")
						mAnimator.Play (action, 0, Random.Range (0, 1f));
					else
						mAnimator.Play (action, 0, 0);
					mAnimator.speed = speed * animationSpeedOffset;
					mPreAction = action;
				}
			}
		}
	}
}
