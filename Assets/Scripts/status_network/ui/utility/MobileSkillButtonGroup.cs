﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MMO
{
	public class MobileSkillButtonGroup : MonoBehaviour
	{

		public Button btn_normal_skill;
		public List<GameObject> obj_skills;
		public List<Button> btn_skills;
		public bool isShow;
		List<Vector3> mDefaultPosList;
		float duration = 0.6f;
		float mTimeToCloseMobileSkillButtonGroup;
		const float DurationToCloseMobileSkillButtonGroup = 5f;
		MMOUnitSkill mUnitSkill;

		void Awake ()
		{
			mDefaultPosList = new List<Vector3> ();
			btn_normal_skill.onClick.AddListener (OnNormalAttack);
			btn_normal_skill.gameObject.SetActive (true);
			btn_skills = new List<Button> ();
			for(int i=0;i< obj_skills.Count;i++){
				btn_skills.Add (obj_skills[i].GetComponentInChildren<Button>());
			}
			for (int i = 0; i < btn_skills.Count; i++) {
				mDefaultPosList.Add (btn_skills [i].transform.localPosition);
				btn_skills [i].transform.localPosition = Vector3.zero;
			}
		}

		void Update ()
		{
			//TODO to update the time on sub skill button.
			if (mTimeToCloseMobileSkillButtonGroup < Time.time && isShow) {
				HideSkills ();
			}
		}

		public void Init (MMOUnitSkill unitSkill)
		{
			this.mUnitSkill = unitSkill;
			SetSkillDatas (this.mUnitSkill);
		}

		public void ShowSkills ()
		{
			for (int i = 0; i < btn_skills.Count; i++) {
				btn_skills [i].transform.DORotate (new Vector3 (0, 0, 180f), duration);
				btn_skills [i].transform.DOLocalMove (mDefaultPosList [i], duration);
				Image image = btn_skills [i].GetComponent<Image> ();
				image.enabled = true;
				image.raycastTarget = false;
				image.DOFade (1f, duration).OnComplete (() => {
					image.raycastTarget = true;
				});
			}
			isShow = true;
		}

		public void HideSkills ()
		{
			for (int i = 0; i < btn_skills.Count; i++) {
				btn_skills [i].transform.DORotate (new Vector3 (0, 0, 0), duration);
				btn_skills [i].transform.DOLocalMove (new Vector3 (0, 0, 0), duration);
				Image image = btn_skills [i].GetComponent<Image> ();//.enabled = false;
				image.DOFade (0f, duration).OnComplete (() => {
					image.raycastTarget = false;
				});
			}
			isShow = false;
		}

		MMOUnitSkill mMMOUnitSkill;
		static int skillStartIndex = 4;

		void SetSkillDatas (MMOUnitSkill unitSkill)
		{
			//TODO get normal attack
			//TODO get skill
			this.mMMOUnitSkill = unitSkill;
			List<SkillBase> skills = unitSkill.skillList;
			for (int i = skillStartIndex; i < skills.Count; i++) {
				SkillBase sb = skills [i];
				Button btnSkill = btn_skills [i - skillStartIndex];
				Sprite iconSprite = ResourcesManager.Instance.GetSkillIcon (sb.mSkill.id);
				MobileSkillButton mobileSkillButton = obj_skills[i - skillStartIndex].gameObject.GetOrAddComponent<MobileSkillButton> ();
				mobileSkillButton.InitSkillButton (iconSprite, 3f, sb, OnSkill);
			}
		}

		//TODO 需要使用实际的技能id
		void OnSkill (SkillBase skillBase)
		{
			mTimeToCloseMobileSkillButtonGroup = Time.time + DurationToCloseMobileSkillButtonGroup;
			if (skillBase.Play ()) {
				mMMOUnitSkill.mmoUnit.unitAnimator.SetTrigger (AnimationConstant.UNIT_ANIMATION_PARAMETER_CAST);
			}
		}

		void OnNormalAttack ()
		{
			mTimeToCloseMobileSkillButtonGroup = Time.time + DurationToCloseMobileSkillButtonGroup;
			if (!isShow) {
				ShowSkills ();
				return;
			}
			//need a area to place the config at user handled skill.
			//this is not in default mmorpg.
			//这里就是动作mmo和传统mmo之间的区别.
			if (mmoUnitSkill.mmoUnit.unitAnimator.IsInState (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK3)) {
				if (mmoUnitSkill.skillList [0].Play ()) {
					mMMOUnitSkill.mmoUnit.unitAnimator.SetTrigger (mmoUnitSkill.skillList [0].mUnitSkill.anim_name);
				}
			} else if (mmoUnitSkill.mmoUnit.unitAnimator.IsInState (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK2)) {
				if (mmoUnitSkill.skillList [1].Play ()) {
					mMMOUnitSkill.mmoUnit.unitAnimator.SetTrigger (mmoUnitSkill.skillList [1].mUnitSkill.anim_name);
				}
			} else if (mmoUnitSkill.mmoUnit.unitAnimator.IsInState (AnimationConstant.UNIT_ANIMATION_PARAMETER_ATTACK1)) {
				if (mmoUnitSkill.skillList [2].Play ()) {
					mMMOUnitSkill.mmoUnit.unitAnimator.SetTrigger (mmoUnitSkill.skillList [2].mUnitSkill.anim_name);
				}
			} else {
				if (mmoUnitSkill.skillList [3].Play ()) {
					mMMOUnitSkill.mmoUnit.unitAnimator.SetTrigger (mmoUnitSkill.skillList [0].mUnitSkill.anim_name);
				}
			}
		}
	}
}