using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolume : MonoBehaviour {
		public Volume volume;

		public Bloom bloom;
		public Vignette vignette;

		private float speed = 2f;
		
		private float defaultBloomValue;
		private float defaultVignetteValue;
		
		private void OnEnable() {
			if (volume.profile.TryGet(out bloom)) {
				bloom.intensity.overrideState = true;
				defaultBloomValue = bloom.intensity.value;
			}
			
			if (volume.profile.TryGet(out vignette)) {
				vignette.intensity.overrideState = true;
				defaultVignetteValue = vignette.intensity.value;
			}
		}

		public void StartPlayerDamageCast() {
			if (volume.profile.TryGet(out bloom)) {
				bloom.intensity.overrideState = true;
			}

			if (volume.profile.TryGet(out vignette)) {
				vignette.intensity.overrideState = true;
			}
				
			StartCoroutine(IntensityProcess());
		}

		private float time;
		IEnumerator IntensityProcess() {
			time = 0;
			while (time < Player.instance.damagePassTime / 2) {
				time += Time.deltaTime;
				if (!bloom || !vignette) yield return null;
				bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 3f, Time.deltaTime);
				vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 1, Time.deltaTime);
				yield return null;
			}

			while (time <= Player.instance.damagePassTime) {
				time += Time.deltaTime;
				if (!bloom || !vignette) yield return null;
				bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, defaultBloomValue, Time.deltaTime);
				vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, defaultVignetteValue, Time.deltaTime);
				yield return null;
			}
		}
		
		
}
