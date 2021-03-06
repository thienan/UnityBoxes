﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class HealthBar : MonoBehaviour {
	public Living unit;
	public Color goodColor;
	public Color badColor;

	Image image;

	HealthBar() {
	}

	void Reset() {
		goodColor = Color.Lerp(Color.green, new Color(0,255,0,0), 0.6f);
		badColor = Color.Lerp(Color.red, new Color(255,0,0,0), 0.6f);
	}

	void Start() {
		image = GetComponent<Image> ();

		// try finding the Unit
		if (unit == null) {
			// children
			unit = GetComponentInChildren<Living> ();
		}
		if (unit == null && transform.parent != null) {
			// parent
			unit = GetComponentInParent<Living> ();
			if (unit == null) {
				// siblings
				unit = transform.parent.GetComponentInChildren<Living> ();
			}
		}
	}

	void Update() {
		var ratio = unit.Health / unit.MaxHealth;

		// set color
		var color = Color.Lerp(badColor, goodColor, ratio);
		image.color = color;

		// set size
		image.fillAmount = ratio;
	}
}
