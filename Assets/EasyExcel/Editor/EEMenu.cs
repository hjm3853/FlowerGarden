using EnumDef;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EasyExcel
{
	public static class EEMenu
	{		
		/// <summary>
		/// 임포트 누를때 메소드
		/// </summary>
		[MenuItem(@"Tools/EasyExcel/Import", false, 100)]
		public static void ImportFolder()
		{
			var historyExcelPath = EditorPrefs.GetString(EEConverter.excelPathKey);
			if (string.IsNullOrEmpty(historyExcelPath) || !Directory.Exists(historyExcelPath))
			{
				var fallbackDir = Environment.CurrentDirectory + "/Assets/EasyExcel/Example/ExcelFiles";
				historyExcelPath = Directory.Exists(fallbackDir) ? fallbackDir : Environment.CurrentDirectory;
			}

			var excelPath =  EditorUtility.OpenFolderPanel("파일을 선택하세요", historyExcelPath, "");
			// var excelInd = EditorUtility.OpenFolderPanel("파일을 선택하세요", historyExcelPath, "");
			if (string.IsNullOrEmpty(excelPath))
				return;
			
			EditorPrefs.SetString(EEConverter.excelPathKey, excelPath);
			EEConverter.GenerateCSharpFiles(excelPath, Environment.CurrentDirectory + "/" + EESettings.Current.GeneratedScriptPath);
		}

		public static void ImportIndivisual(string excelPath,string[] paths)
		{
			EEConverter.GenerateCSharpFiles(excelPath, Environment.CurrentDirectory + "/" + EESettings.Current.GeneratedScriptPath,paths);
		}

		
		[MenuItem(@"Tools/EasyExcel/Clean", false, 101)]
		public static void Clean()
		{
			EditorPrefs.SetBool(EEConverter.csChangedKey, false);

			DeleteCSFolder();
			DeleteScriptableObjectFolder();

			AssetDatabase.Refresh();
			//결과확인
			EditorWindow.GetWindow<ResultWindow>().Show();
			EditorWindow.GetWindow<ResultWindow>().Focus();
			EditorWindow.GetWindow<ResultWindow>().CheckStr = "DB가 모두 초기화 되었습니다.";
		}

		[DidReloadScripts(1)]
		private static void OnScriptsReloaded()
		{ 
			bool isOpen = EditorWindow.HasOpenInstances<OdinEasyExcelMenuTree>();
			if (isOpen)   
			{
				OdinEasyExcelMenuTree.OpenWindow();
				OdinEasyExcelMenuTree.kTotalMenu.GetSetting().LoadPathSetting();
				OdinEasyExcelMenuTree.kTotalMenu.GetTable().GetExelDataAll();
			
			}
			//json생성
			if (File.Exists(Application.persistentDataPath + "/" + "CreateSTOList.json"))
			{
				var json=File.ReadAllText(Application.persistentDataPath + "/" + "CreateSTOList.json"); 
				var stoList=JsonConvert.DeserializeObject<List<string>>(json);
			
				if (stoList != null && stoList.Count > 0)
				{
					EEConverter.GenerateScriptableObjects( OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientAssetPath, DBType.Client);
				}
			}		

		}

		private static void DeleteCSFolder()
		{
			if (Directory.Exists(EESettings.Current.GeneratedScriptPath))
				Directory.Delete(EESettings.Current.GeneratedScriptPath, true);

			string csMeta = null;
			if (EESettings.Current.GeneratedScriptPath.EndsWith("/") || EESettings.Current.GeneratedScriptPath.EndsWith("\\"))
				csMeta = EESettings.Current.GeneratedScriptPath.Substring(0, EESettings.Current.GeneratedScriptPath.Length - 1) + ".meta";
			if (!string.IsNullOrEmpty(csMeta) && File.Exists(csMeta))
				File.Delete(csMeta);
		}

		private static void DeleteScriptableObjectFolder()
		{
			if (Directory.Exists(EESettings.Current.GeneratedClientAssetPath))
				Directory.Delete(EESettings.Current.GeneratedClientAssetPath, true);

			string asMeta = null;
			if (EESettings.Current.GeneratedClientAssetPath.EndsWith("/") || EESettings.Current.GeneratedClientAssetPath.EndsWith("\\"))
				asMeta = EESettings.Current.GeneratedClientAssetPath.Substring(0, EESettings.Current.GeneratedClientAssetPath.Length - 1) + ".meta";
			if (!string.IsNullOrEmpty(asMeta) && File.Exists(asMeta))
				File.Delete(asMeta);
		}
		
	}
}