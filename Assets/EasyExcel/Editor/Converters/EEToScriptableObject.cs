using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EasyExcel
{
	/// <summary>
	///     Excel Converter
	/// </summary>
	public static partial class EEConverter
	{

		//override
		public static void GenerateScriptableObjects( string assetPath, EnumDef.DBType type)
		{
			assetPath = assetPath.Replace("\\", "/");
			//오딘설정파일 가져오기
			string[] filePaths =OdinEasyExcelMenuTree.LoadSelectPath();

			var count = 0;
			for (var i = 0; i < filePaths.Length; ++i)
			{
				var filePath = filePaths[i].Replace("\\", "/");
				if (!IsExcelFile(filePath)) continue;
				ToScriptableObject(filePath, assetPath, type);
				//1 x 100 / 2 = 50
			
				EditorUtility.DisplayCancelableProgressBar("Json 생성",$"{filePath[i]} Json 생성중",100);	
				count++;
			}
			File.Delete(Application.persistentDataPath + "/" + "CreateSTOList.json");
			EditorUtility.DisplayDialog("알림", $"Json {count}개의 파일이 모두 생성되었습니다.", "확인");
			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}


		private static void ToScriptableObject(string excelPath, string outputPath, EnumDef.DBType type)
		{
			var book = EEWorkbook.Load(excelPath);
			if (book == null)
				return;
			foreach (var sheet in book.sheets)
			{
				if (sheet == null)
					continue;
				if (!IsValidSheet(sheet))
					continue;
				//var sheetData = ToSheetData(sheet);
				var sheetData = ToSheetDataRemoveEmptyColumn(sheet);

				//sheetdata가공
				OnUpdateSheetForType(sheetData, type);
			


				ToScriptableObject(excelPath, $"{sheet.name}_{type.ToString()}", outputPath, sheetData);
			}
		}

		/// <summary>
		/// 타입에 따라 필요없는 column삭제
		/// </summary>
		/// <param name="sheetData"></param>
		/// <param name="type"></param>
		private static void OnUpdateSheetForType(SheetData sheetData, EnumDef.DBType type)
		{
			var typeList = OnGetNewTypeList(sheetData);
			int currColumnCount = typeList.Count - 1;
			for (int i = sheetData.ColumnCount - 1; i >= 0; i--)
			{

				EnumDef.DBType currentDBType = (EnumDef.DBType) typeList[currColumnCount];
				//dbtype이 현재 dbtype이 아니면
				if (currentDBType != type)
				{
					//dbtype이 both도 아니면 삭제 //both면 삭제 안함
					if (currentDBType != EnumDef.DBType.Both)
					{
						for (int j = sheetData.RowCount - 1; j >= 0; j--)
						{
							sheetData.Table[j].RemoveAt(i);
						}

					}
				}


				currColumnCount -= 1;
			}

			sheetData.ColumnCount = sheetData.Table[0].Count;
		}

		

		private static List<int> OnGetNewTypeList(SheetData sheetData)
		{
			var tempList = new List<int>();
			var originList = sheetData.Table[OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataColumn];
			for (int i = 0; i < originList.Count; i++)
			{
				tempList.Add(int.Parse(originList[i]));
			}

			return tempList;
		}

	
		private static void ToScriptableObject(string excelPath, string sheetName, string outputPath, SheetData sheetData)
		{
			var timeLapse = new Stopwatch();
			timeLapse.Start();
			string fileName = Path.GetFileName(excelPath);
				var sheetClassName = EESettings.Current.GetSheetClassName(fileName, sheetName);
				string path = string.Empty;
				if (sheetClassName.Contains("Server"))
				{
					path =OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath+ sheetClassName + ".cs";
				}

				if (sheetClassName.Contains("Client"))
				{
					path =OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath+ sheetClassName + ".cs";
				}
				
				if (!File.Exists(path))
				{
					//todo Log.Warning으로 대체
					Debug.LogWarning("path");
					Debug.LogWarning("스크립트가 존재하지 않아서 STO를 생성하지않습니다.");
					return;	
				}
				var dataCollect = new List<EERowData>();
				var className = EESettings.Current.GetRowDataClassName(fileName, sheetName, true);
				var dataType = Type.GetType(className);
				if (dataType == null)
				{
					var assemblies = AppDomain.CurrentDomain.GetAssemblies();
					foreach (var assembly in assemblies)
					{
						dataType = assembly.GetType(className);
						if (dataType != null)
							break;
					}
				}

				if (dataType == null)
				{
					EELog.LogError(className + " not exist !");
					return;
				}

				var dataCtor = dataType.GetConstructor(new[] {typeof(List<List<string>>), typeof(int), typeof(int)});
				if (dataCtor == null)
					return;
				var keySet = new HashSet<object>();
				for (var row =OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kPropertiesColumn; row < sheetData.RowCount; ++row)
				{
					for (var col = 0; col < sheetData.ColumnCount; ++col)
					{
						sheetData.Set(row, col, sheetData.Get(row, col).Replace("\n", "\\n"));
					}

					var inst = dataCtor.Invoke(new object[] {sheetData.Table, row, 0}) as EERowData;
					if (inst == null)
						continue;

					var key = inst.GetKeyFieldValue();
					if (key == null)
					{
						EELog.LogError("Sheet에 입력된 Key가 null입니다 " + sheetName);
						continue;
					}

					if (key is int i && i == 0)
						continue;

					if (key is string s && string.IsNullOrEmpty(s))
						continue;

					if (!keySet.Contains(key))
					{
						dataCollect.Add(inst);
						keySet.Add(key);
					}
					else
					{
#if LOG
						Log.Error($"{sheetName}Sheet에 Key가 중복되거나 0인 Column이 있습니다. 키 : <{key}>",Log.Account.Default);
#endif
					} 
						
				}

				if (sheetName.Contains("Client"))
				{
					CreateJson($"{sheetClassName}", dataCollect,OdinEasyExcelMenuTree.kTotalMenu.GetSetting().GetClientAssetSavePath());
					timeLapse.Stop();
#if LOG
					Log.Message($"<{sheetClassName} Json생성시간> : {timeLapse.ElapsedMilliseconds*0.001f} sec");
#endif
					return;
				}

				if (sheetName.Contains("Server"))
				{
					CreateJson($"{sheetClassName}", dataCollect,OdinEasyExcelMenuTree.kTotalMenu.GetSetting().GetServerAssetPath());
					timeLapse.Stop();
#if LOG
					Log.Message($"<{sheetClassName} Json생성시간> : {timeLapse.ElapsedMilliseconds.ToString()}");
#endif
					return;
				}
			
		}

		

		/// <summary> 서버데이터 Json생성 </summary>
		
		private static void CreateJson(string className, List<EERowData> dataCollect,string savePath)
		{

			className=RemoveOverwrite(className);
			//서버Json만 세팅
		//	if (className.Split('_')[1] != "Server") return;
			//서버데이터 리스트세팅
			//var list = new List<EERowData>();
			// for (int i = 0; i < dataCollect.Count; i++)
			// {
			// 	list.Add(dataCollect.GetData(i));
			// }

			// Json생성
			var json = JsonConvert.SerializeObject(dataCollect, Formatting.Indented);
			if (!Directory.Exists(OdinEasyExcelMenuTree.kTotalMenu.GetSetting(). GetSaveCppPath()))
			{
				Directory.CreateDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().GetSaveCppPath());
			}
			string JsonFilePath = $"{savePath}{className}.json";
			StreamWriter sw = File.CreateText(JsonFilePath);
			sw.WriteLine(json);
			sw.Close();
		}

		private static string RemoveOverwrite(string str)
		{
			var split=str.Split('_');
			var disArray=split.Distinct().ToArray();
			disArray[disArray.Length - 1] ="_"+ disArray[disArray.Length - 1];
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < disArray.Length; i++)
			{
				sb.Append(disArray[i]);
			}

			return sb.ToString();
		}

		


	}


}