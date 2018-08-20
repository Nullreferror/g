﻿using System;
using UnityEngine;

public class CardBehaviour : MonoBehaviour {
	
	private enum AnimationState {

		Idle,
		Converging,
		FlyingAway,
		Revealing

	}
	
	private const float animationDuration = 0.4f;
	
	public float SwipeThreshold = 1.0f;
	public Vector3 SnapPosition;
	public Vector3 SnapRotationAngles;
	
	public CardModel Card { private get; set; }
	public Game Controller { private get; set;  }
	
	private Vector3 dragStartPosition;
	private Vector3 dragStartPointerPosition;
	private Vector3 animationStartPosition;
	private Vector3 animationStartRotationAngles;
	private float animationStartTime;
	private AnimationState animationState = AnimationState.Idle;

	private void Start() {
		animationStartPosition = transform.position;
		animationStartRotationAngles = transform.eulerAngles;
		animationStartTime = Time.time;
		animationState = AnimationState.Revealing;
	}
	
	private void Update() {
		// Animate card by interpolating translation and rotation, destroy swiped cards
		if (animationState != AnimationState.Idle) {
			float animationProgress = (Time.time - animationStartTime) / animationDuration;
			float scaledProgress = ScaleProgress(animationProgress);
			if (scaledProgress > 1.0f || animationProgress > 1.0f) {
				transform.position = SnapPosition;
				transform.eulerAngles = SnapRotationAngles;
				if (animationState == AnimationState.FlyingAway) {
					Destroy(gameObject);
				}
				else {
					animationState = AnimationState.Idle;
				}
			}
			else {
				transform.position = Vector3.Lerp(animationStartPosition, SnapPosition, scaledProgress);
				transform.eulerAngles = Vector3.Lerp(animationStartRotationAngles, SnapRotationAngles, scaledProgress);
			}
		}
	}
	
	private void OnMouseDown() {
		animationState = AnimationState.Idle;
		dragStartPosition = transform.position;
		dragStartPointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
	private void OnMouseDrag() {
		Vector3 displacement = Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragStartPointerPosition;
		displacement.z = 0.0f;
		transform.position = dragStartPosition + displacement;
	}
	
	private void OnMouseUp() {
		animationStartPosition = transform.position;
		animationStartTime = Time.time;
		if (transform.position.x < SnapPosition.x - SwipeThreshold) {
			Card.PerformLeftDecision();
			Controller.SpawnCard();
			Vector3 displacement = animationStartPosition - SnapPosition;
			SnapPosition += displacement.normalized
			                * Util.OrthoCameraWorldDiagonalSize(Camera.main)
			                * 2.0f;
			SnapRotationAngles = transform.eulerAngles;
			animationState = AnimationState.FlyingAway;
		}
		else if (transform.position.x > SnapPosition.x + SwipeThreshold) {
			Card.PerformRightDecision();
			Controller.SpawnCard();
			Vector3 displacement = animationStartPosition - SnapPosition;
			SnapPosition += displacement.normalized
			                * Util.OrthoCameraWorldDiagonalSize(Camera.main)
			                * 2.0f;
			SnapRotationAngles = transform.eulerAngles;
			animationState = AnimationState.FlyingAway;
		}
		else {
			animationState = AnimationState.Converging;
		}
	}

	private float ScaleProgress(float animationProgress) {
		switch (animationState) {
			case AnimationState.Converging:
				return 0.15f * Mathf.Pow(animationProgress, 3.0f)
				       - 1.5f * Mathf.Pow(animationProgress, 2.0f)
				       + 2.38f * animationProgress;
			case AnimationState.FlyingAway:
				return 1.5f * Mathf.Pow(animationProgress, 3.0f)
				      + 0.55f * animationProgress;
			default:
				return animationProgress;
		}
	}
	
}
