﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Displays arrows toward all "Tracked" objects outside of the viewport
/// </summary>
public class ObjectTracker : MonoBehaviour {
	public static ObjectTracker Instance {
		get;
		private set;
	}

	public Image arrowPrefab;
	public Canvas trackerCanvas;
	HashSet<Tracked> onCameraObjects, offCameraObjects;

	ObjectTracker() {
		Instance = this;
		Reset ();
	}

	void Reset() {
		onCameraObjects = new HashSet<Tracked> ();
		offCameraObjects = new HashSet<Tracked> ();
	}

	void Update() {
		foreach (var tracked in offCameraObjects) {
			if (tracked.isActiveAndEnabled) {
				EnableArrow (tracked);
				DrawArrow (tracked);
			} else {
				DisableArrow (tracked);
			}
		}
	}

	public void UpdateTrackStatus(Tracked go) {
		if (go.IsOnCamera) {
			onCameraObjects.Add (go);
			offCameraObjects.Remove (go);

			DisableArrow (go);
		} else {
			onCameraObjects.Remove (go);
			offCameraObjects.Add(go);
		}
	}

	public void StopTracking(Tracked go) {
		DisableArrow (go);
		onCameraObjects.Remove (go);
		offCameraObjects.Remove (go);
		if (go.ArrowImage != null) {
			Destroy (go.ArrowImage);
		}
	}

	void EnableArrow(Tracked go) {
		if (go.ArrowImage == null) {
			var arrow = (Image)Instantiate(arrowPrefab, trackerCanvas.transform);
			go.ArrowImage = arrow;
		}
		go.ArrowImage.enabled = true;
	}

	void DisableArrow(Tracked go) {
		if (go.ArrowImage != null) {
			go.ArrowImage.enabled = false;
		}
	}

	void DrawArrow(Tracked go) {
		// convert from viewport range (spanning [0 to 1]) to [-1 to 1]
		var center = new Vector3(0.5f, 0.5f, 0);
		var viewPoint = (go.ViewportPoint - center) * 2;
		viewPoint.z = 0;
		viewPoint.Normalize ();

		var angle = Mathf.Atan2 (viewPoint.y, viewPoint.x) * Mathf.Rad2Deg;
		var trans = go.ArrowImage.GetComponent<RectTransform> ();
		var canvasTrans = trackerCanvas.GetComponent<RectTransform> ();

		trans.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		trans.position = new Vector3(canvasTrans.rect.width/2 * (1+viewPoint.x), canvasTrans.rect.height/2 * (1+viewPoint.y), 0);
	}

}
