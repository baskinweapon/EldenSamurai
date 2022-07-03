using UnityEngine;
using System;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "SettingsAsset", menuName = "Game/Settings", order = 0)]
public class SettingsAsset : ScriptableObject {
	[SerializeField]
	public GameSettings serializable;
	
	[Header("other info")]
	public string path;
	public string saveName = "save.save";


	[Header("debug")]
	public bool debugMode;

	private void OnEnable() {
		path = Application.streamingAssetsPath + $"/settings/{saveName}";
	}


	public bool FileCheck() {
		if (string.IsNullOrEmpty(path)) OnEnable();

		bool b = File.Exists(path);
		if (!b) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}

			File.Create(path).Close();
		}

		return b;
	}
	
	public void CreateNewSave() {
		SetDefaultSave();

		string repath = path.Replace(saveName, $"broken_save_{DateTime.Now:yy_MM_dd_hhmmss}.th");

		File.Move(path, repath);
	}

	public void SetDefaultSave() {
		
	}


	public void LoadFromFile() {
		if (!FileCheck()) {
			SetDefaultSave();
		} else {
			string json;
			using (StreamReader reader = new StreamReader(path)) {
				json = reader.ReadToEnd();
			}

			try {
				serializable = JsonUtility.FromJson<GameSettings>(json);
			}
			catch (Exception e) {
				Console.WriteLine(e);
				CreateNewSave();
			}
		}
	}
	
	public void ApplyAssetToFile() {
#if UNITY_EDITOR
#endif
		SaveToFile();
	}


	public void SaveToFile() {
		FileCheck();

		using (StreamWriter writer = new StreamWriter(path)) {
			var json = JsonUtility.ToJson(serializable, true);
			writer.Write(json);
			writer.Flush();
		}
		
		Debug.Log("File Saved");
	}


	public void SaveProgress() {
		ApplyAssetToFile();
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(SettingsAsset))]
public class SettingsAssetEditor : Editor {
	public override void OnInspectorGUI() {
		var me = target as SettingsAsset;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("LoadFromFile")) {
			me.LoadFromFile();
		}

		if (GUILayout.Button("Open settings Folder")) {
			EditorUtility.RevealInFinder(me.path);
		}

		if (GUILayout.Button("SaveToFile")) {
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
			me.ApplyAssetToFile();
		}

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		base.OnInspectorGUI();

		EditorGUILayout.Space();


		if (GUILayout.Button("ClearAll")) {
			me.SetDefaultSave();
		}
		
		EditorGUILayout.Space();


		if (GUILayout.Button("SaveAsset")) {
			AssetDatabase.Refresh();
			EditorUtility.SetDirty(target);
			AssetDatabase.SaveAssets();
		}
		
		EditorGUILayout.Space();
	}
}

#endif
