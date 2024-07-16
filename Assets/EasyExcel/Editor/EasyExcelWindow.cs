using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Timeline;
using FileMode = System.IO.FileMode;


namespace EasyExcel
{
	[ShowOdinSerializedPropertiesInInspector]
	[Serializable]
	public class OdinEasyExcelMenuTree : OdinMenuEditorWindow
	{
		public static OdinEasyExcelMenuTree kTotalMenu;

		/// <summary>가장 최근 생성한 테이블 목록 저장</summary>
		public static void SaveLatestCreateDB(List<string> successList)
		{
			var json = JsonConvert.SerializeObject(successList, Formatting.Indented);
			var dir = Application.persistentDataPath;
			string fileName = "EasyExcelLatestCreateDBFile.json";
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			StreamWriter sw = File.CreateText(dir + "/" + fileName);
			sw.WriteLine(json);
			sw.Close();
		}

		/// <summary>생성할 테이블 엑셀리스트 저장</summary>
		public static void SaveSelectPath(string[] saveStr)
		{
			var json = JsonConvert.SerializeObject(saveStr, Formatting.Indented);
			var dir = Application.persistentDataPath;
			string fileName = "EasyExcelSettingFile.json";
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			StreamWriter sw = File.CreateText(dir + "/" + fileName);
			sw.WriteLine(json);
			sw.Close();
		}

		/// <summary>생성할 테이블 엑셀리스트 불러오기</summary>
		public static string[] LoadSelectPath()
		{
			string JsonFilePath = $"{Application.persistentDataPath}/EasyExcelSettingFile.json";
			var allStr = File.ReadAllText(JsonFilePath);
			var data = JsonConvert.DeserializeObject<string[]>(allStr);
			return data;
		}


		/// <summary>최근생성한 Table List가져오기</summary>
		public static List<string> LoadLatestDBList()
		{
			string JsonFilePath = $"{Application.persistentDataPath}/EasyExcelLatestCreateDBFile.json";
			if (File.Exists(JsonFilePath) == false)
			{
				SaveLatestCreateDB(new List<string>());
			}

			var allStr = File.ReadAllText(JsonFilePath);
			var data = JsonConvert.DeserializeObject<List<string>>(allStr);
			return data;
		}

		[MenuItem("GameTool/Excel Exporter", false, priority:4)]
		public static void OpenWindow()
		{
			kTotalMenu = GetWindow<OdinEasyExcelMenuTree>();
			kTotalMenu.maxSize = new Vector2(1280, 720);
			kTotalMenu.Show();
			kTotalMenu.ForceMenuTreeRebuild();
			kTotalMenu.GetSetting().LoadPathSetting();
			kTotalMenu.GetSetting().LoadCreateCppOption();
			kTotalMenu.GetTable().GetExelDataAll();

		}

		/// <summary>메뉴트리 생성</summary>
		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			tree.Add("DB", CreateInstance<EasyExcelWindow>());
			tree.Add("Setting", CreateInstance<EasyExcelSetting>());
			return tree;
		}

		/// <summary>세팅 윈도우 가져오기</summary>
		public EasyExcelSetting GetSetting()
		{
			return (EasyExcelSetting) kTotalMenu.MenuTree.GetMenuItem("Setting").Value;
		}

		/// <summary>테이블윈도우 가져오기</summary>
		public EasyExcelWindow GetTable()
		{
			return (EasyExcelWindow) kTotalMenu.MenuTree.GetMenuItem("DB").Value;
		}
	}


	public class IPData
	{
		public string ServerIP;
		public string DBIP;

		public IPData(string _serverIP, string _dbIP)
		{
			ServerIP = _serverIP;
			DBIP = _dbIP;
		}
	}


	//DB세팅윈도우
	public class EasyExcelWindow : OdinEditorWindow
	{
		private bool isCheckAll;

		[PropertyOrder(101)] [TableList] [BoxGroup("엑셀 리스트")]
		public List<EasyExcelSetting.OdinExcelData> ExcelDataList = new List<EasyExcelSetting.OdinExcelData>();


		private string[] selectedPath;

		public string[] SelectedPath
		{
			get => selectedPath;
			set => selectedPath = value;
		}

	
		[PropertyOrder(100)][Button(30)][BoxGroup("엑셀 리스트")][GUIColor(0.4129584f, 0.6037736f, 0.4244054f, 1f)]
		public void 모두선택()
		{
			if (isCheckAll)
			{
				isCheckAll = false;
				for (int i = 0; i < ExcelDataList.Count; i++)
				{
					ExcelDataList[i].선택 = false;
				}
			}
			else
			{
				isCheckAll = true;
				for (int i = 0; i < ExcelDataList.Count; i++)
				{
					ExcelDataList[i].선택 = true;
				}
			}
		}
		
		[PropertyOrder(99)]
		[Button("엑셀데이터 업데이트", 30)]
		[TableColumnWidth(50, false)]
		[HorizontalGroup("메뉴")]
		public void GetExelDataAll()
		{
			ExcelDataList.Clear();
			string[] paths = GetFolderExcelPaths();
			for (int i = 0; i < paths.Length; i++)
			{
				if (paths[i].Contains(".meta")) continue;
				if (paths[i].Contains("~$")) continue;
				ExcelDataList.Add(new EasyExcelSetting.OdinExcelData(false, paths[i]));
			}
		}

		[PropertyOrder(99)]
		[Button("테이블생성", 30)]
		[TableColumnWidth(50, false)]
		[HorizontalGroup("메뉴")]
		public void CreateTable()
		{
			var pathList = new List<string>();
			for (int i = 0; i < ExcelDataList.Count; i++)
			{
				if (ExcelDataList[i].선택)
				{
					pathList.Add(ExcelDataList[i].엑셀위치);
				}
			}

			if (pathList.Count == 0) return;
			OdinEasyExcelMenuTree.SaveSelectPath(pathList.ToArray());

			var askStr = string.Empty;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < pathList.Count; i++)
			{
				sb.Append($"{pathList[i]}\n");
			}

			sb.Append("해당 Table을 생성할까요?");
			
			bool decision = EditorUtility.DisplayDialog(
				"알림", // title
				sb.ToString(), // description
				"예", // OK button
				"아니오" // Cancel button
			);
			if (decision)
			{
				var excelPath = Environment.CurrentDirectory + "/Assets/Excel";

				EEConverter.GenerateCSharpFiles(excelPath, Environment.CurrentDirectory + "/" + EESettings.Current.GeneratedScriptPath);
				AssetDatabase.Refresh();
			}
			else
			{
				Debug.Log("Cancel");
			}
			
		}

		private string SetDBCreateNoticeString(List<string> pathList)
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append($"총 {pathList.Count}개의 엑셀 데이터를 업로드할까요?");
			for (int i = 0; i < pathList.Count; i++)
			{
				string str = pathList[i].Replace("\\", "/");

				strBuilder.Append($"\n{str}");
			}

			return strBuilder.ToString();
		}


		
		[PropertyOrder(99)]
		[Button("데이터 모두지우기", 30)]
		[TableColumnWidth(50, false)]
		[GUIColor(1, 0, 0)]
		[HorizontalGroup("메뉴")]
		public void Clean()
		{
			bool decision = EditorUtility.DisplayDialog(
				"알림", // title
				"DB를 모두 삭제할까요?", // description
				"예", // OK button
				"아니오" // Cancel button
			);
			if (decision)
			{
				DeleteAll();
			}
			else
			{
				Debug.Log("Cancel");
			}

			return;
		}

		private void DeleteAll()
		{
			DeleteDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath);
			DeleteDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath);
			DeleteDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientAssetPath);
			DeleteDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerAssetPath);
			DeleteDirectory(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveCppPath);
			EditorUtility.DisplayDialog("알림", "데이터/폴더가 모두 삭제되었습니다.", "확인");
		}

		private void DeleteDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}
		}

		private string[] GetFolderExcelPaths()
		{
			var historyExcelPath = Environment.CurrentDirectory + "/Assets/Excel";
			if (string.IsNullOrEmpty(historyExcelPath) || !Directory.Exists(historyExcelPath))
			{
				var fallbackDir = Environment.CurrentDirectory + "/Assets/EasyExcel/Example/ExcelFiles";
				historyExcelPath = Directory.Exists(fallbackDir) ? fallbackDir : Environment.CurrentDirectory;
			}

			var value = Directory.GetFiles(historyExcelPath);
			return value;
		}
	}


//이지엑셀설정 윈도우
	[Serializable]
	public class EasyExcelSetting : OdinEditorWindow
	{
		#region Odin WindowView

		[BoxGroup("Column설정")] [LabelText("Column")] [Title("[자료형 Column]", "ex)int, Long.. etc")]
		public int kPropertyType = 1;

		[BoxGroup("Column설정")] [LabelText("Column")] [Title("[Server/Client 설정]", "ex)0=None, 1=Client, 2=Server, 3=Both")]
		public int kDataColumn = 2;

		[BoxGroup("Column설정")] [LabelText("Column")] [Title("[데이터 Column]", "ex)UID:key, ItemName.. etc")]
		public int kDataType = 3;

		[BoxGroup("Column설정")] [LabelText("Column")] [Title("[데이터 시작  Column]", "ex)240006030001 유혼반지 Icon_Ring_03_01.. etc")]
		public int kPropertiesColumn = 4;
		
		[BoxGroup("Column설정")][Title("Table생성시 테이블데이터 헤더도 함께 생성")][LabelText("테이블데이터 헤더생성")][OnValueChanged("SetCppOption")]
		public bool isCreateCpp;

		[HorizontalGroup("SaveClientCsPath", 800)] [LabelText("클라이언트 저장 CS 경로")]
		public string SaveClientCsPath;


		private void SetCppOption()
		{
			SaveCreateCppOption();
		}

		public void LoadCreateCppOption()
		{
			string savedPath = Application.persistentDataPath + "/CreateCppOption.json";
			//기존저장 파일이 없으면
			if (!File.Exists(savedPath))
			{
				SaveCreateCppOption();
			}
			//로드
			var allStr = File.ReadAllText(savedPath);
			bool loadValue = JsonConvert.DeserializeObject<bool>(allStr);
			isCreateCpp = loadValue;
		}
		

		private void SaveCreateCppOption()
		{
			var dir = Application.persistentDataPath;
			string fileName = "CreateCppOption.json";
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			StreamWriter sw = File.CreateText(dir + "/" + fileName);
			var json = JsonConvert.SerializeObject(isCreateCpp, Formatting.Indented);
			sw.WriteLine(json);
			sw.Close();
		}


		[HorizontalGroup("SaveClientCsPath", 100)]
		[Button("경로선택")]
		public void SetSaveClientCsPath()
		{
			var path = EditorUtility.OpenFolderPanel("클라이언트CS를 저장할 폴더를 선택하세요", SaveClientCsPath, "");
			if (string.IsNullOrEmpty(path)) return;
			SaveClientCsPath = path + "/";
			SaveSetting();
		}

		[HorizontalGroup("SaveServerCsPath", 800)] [LabelText("서버 저장 CS 경로")]
		public string SaveServerCsPath;


		[HorizontalGroup("SaveServerCsPath", 100)]
		[Button("경로선택")]
		public void SetSaveServerCsPath()
		{
			var path = EditorUtility.OpenFolderPanel("서버CS를 저장할 폴더를 선택하세요", SaveServerCsPath, "");
			if (string.IsNullOrEmpty(path)) return;
			SaveServerCsPath = path + "/";
			SaveSetting();
		}

		[HorizontalGroup("SaveClientAssetPath", 800)] [LabelText("클라이언트 저장 Asset 경로")]
		public string SaveClientAssetPath;

		[HorizontalGroup("SaveClientAssetPath", 100)]
		[Button("경로선택")]
		public void SetSaveClientAssetPath()
		{
			var path = EditorUtility.OpenFolderPanel("클라이언트 Asset을 저장할 폴더를 선택하세요", SaveClientAssetPath, "");
			if (string.IsNullOrEmpty(path)) return;
			SaveClientAssetPath = path + "/";
			;
			SaveSetting();
		}

		[HorizontalGroup("SaveServerAssetPath", 800)] [LabelText("서버 저장 Asset 경로")]
		public string SaveServerAssetPath;

		[HorizontalGroup("SaveServerAssetPath", 100)]
		[Button("경로선택")]
		public void SetSaveServerAssetPath()
		{
			var path = EditorUtility.OpenFolderPanel("서버 Asset을 저장할 폴더를 선택하세요", SaveServerAssetPath, "");
			if (string.IsNullOrEmpty(path)) return;
			SaveServerAssetPath = path + "/";
			SaveSetting();
		}

		[HorizontalGroup("SaveCppPath", 800)] [LabelText("테이블데이터 헤더 저장 경로")]
		public string SaveCppPath;

		[HorizontalGroup("SaveCppPath", 100)]
		[Button("경로선택")]
		public void SetSaveJsonPath()
		{
			var path = EditorUtility.OpenFolderPanel("서버 Cpp를 저장할 폴더를 선택하세요", SaveCppPath, "");
			if (string.IsNullOrEmpty(path)) return;
			SaveCppPath = path + "/";
			SaveSetting();
		}

		[HorizontalGroup("경로저장삭제버튼")]
		[Button("모두저장", 40)]
		public void SavePathData()
		{
			SaveSetting();
		}

		[HorizontalGroup("경로저장삭제버튼")]
		[Button("설정 초기화", 40), GUIColor(1, 0, 0)]
		public void ResetSetting()
		{
			bool decision = EditorUtility.DisplayDialog(
				"알림", // title
				"모든 설정을 기본값으로 되돌릴까요?", // description
				"예", // OK button
				"아니오" // Cancel button
			);
			if (decision)
			{
				string savedPath = Application.persistentDataPath + "/EasyExcelPathSettings.json";
				if (File.Exists(savedPath))
				{
					File.Delete(savedPath);
				}

				LoadPathSetting();

				EditorUtility.DisplayDialog("알림", "설정이 초기화 되었습니다.", "확인");
			}
			else
			{
				Debug.Log("Cancel");
			}
		}

		#endregion

		public string GetSaveCppPath()
		{
			var paht = SaveCppPath;
			return SaveCppPath;
		}

		public string GetServerAssetPath()
		{
			var path = SaveServerAssetPath;
			return path;
		}

		public string GetClientAssetSavePath()
		{
			return SaveClientAssetPath;
		}
		

		/// <summary>파일저장경로 로드 </summary>
		public void LoadPathSetting()
		{
			string savedPath = Application.persistentDataPath + "/EasyExcelPathSettings.json";
			//기존저장 파일이 없으면
			if (!File.Exists(savedPath))
			{
				//기본값으로세팅후
				EESettings.Current.ResetAll();
				GetPathFromEE();
				//저장
				SaveSetting();
			}

			//로드
			var allStr = File.ReadAllText(savedPath);
			Dictionary<string, string> loadedDic = JsonConvert.DeserializeObject<Dictionary<string, String>>(allStr);
			
			kPropertyType = int.Parse(loadedDic["kPropertyType"]);
			kDataColumn = int.Parse(loadedDic["kDataColumn"]);
			kDataType = int.Parse(loadedDic["kDataType"]);
			kPropertiesColumn = int.Parse(loadedDic["kPropertiesColumn"]);
			//경로값
			var saveClientCsPath=loadedDic.TryGetValue("SaveClientCsPath",out string path0);
			var saveServerCsPath=loadedDic.TryGetValue("SaveServerCsPath",out string path1);
			var saveClientAssetPath=loadedDic.TryGetValue("SaveClientAssetPath",out string path2);
			var saveServerAssetPath=loadedDic.TryGetValue("SaveServerAssetPath",out string path3);
			var saveCppPath=loadedDic.TryGetValue("SaveCppPath",out string path4);

			//없으면 기본경로
			SaveClientCsPath = string.IsNullOrEmpty(path0) ?  Application.dataPath + "/" + "Scripts/SheetData/":path0 ;
			SaveServerCsPath = string.IsNullOrEmpty(path1) ?  Application.dataPath + "/" + "Scripts/SheetData/":path1 ;
			SaveClientAssetPath = string.IsNullOrEmpty(path2) ?Application.dataPath + "/" + "Bundle/Data/": path2 ;
			SaveServerAssetPath = string.IsNullOrEmpty(path3) ? Application.dataPath + "/" + "Resources/SheetData/":path3  ;
			SaveCppPath = string.IsNullOrEmpty(path4) ? SaveCppPath= $"{Application.dataPath}/Resources/SheetData/":path4 ;
			
		}

		public void SaveSetting()
		{
			var dir = Application.persistentDataPath;
			string fileName = "EasyExcelPathSettings.json";
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			var saveDic = new Dictionary<string, string>();
			saveDic.Add("SaveClientCsPath", SaveClientCsPath);
			saveDic.Add("SaveServerCsPath", SaveServerCsPath);
			saveDic.Add("SaveClientAssetPath", SaveClientAssetPath);
			saveDic.Add("SaveServerAssetPath", SaveServerAssetPath);
			saveDic.Add("SaveCppPath", SaveCppPath);
			saveDic.Add("kPropertyType", kPropertyType.ToString());
			saveDic.Add("kDataColumn", kDataColumn.ToString());
			saveDic.Add("kDataType", kDataType.ToString());
			saveDic.Add("kPropertiesColumn", kPropertiesColumn.ToString());


			StreamWriter sw = File.CreateText(dir + "/" + fileName);
			var json = JsonConvert.SerializeObject(saveDic, Formatting.Indented);
			sw.WriteLine(json);
			sw.Close();
		}


		public void SavePath()
		{
			SaveSetting();
		}


		public void GetPathFromEE()
		{
			/*
			NameRowIndex = 3;
			TypeRowIndex = 1;
			DataStartIndex = 4;
			DBKind = 2;
			GeneratedClientAssetPath = "Bundle/Data/";
			GeneratedServerAssetPath="Resources/SheetData/";
			GeneratedScriptPath = "Scripts/SheetData/";
			SheetDataPostfix = "";
			RowDataPostfix = "";
			NameSpace = "SheetData";
			NameSpacePrefix = "EasyExcel_";
			*/
			SaveClientCsPath = Application.dataPath + "/" + "Scripts/SheetData/";
			SaveServerCsPath = Application.dataPath + "/" + "Scripts/SheetData/";
			SaveClientAssetPath = Application.dataPath + "/" + "Bundle/Data/";
			SaveServerAssetPath = Application.dataPath + "/" + "Resources/SheetData/";
			SaveCppPath = $"{Application.dataPath}/Resources/SheetData/";
		}

		public class LatestUpdateDBData
		{
			[TableColumnWidth(30, false)] public bool 선택;
			[TableColumnWidth(650, false)] public string 파일경로;
			[TableColumnWidth(150, false)] public Type kType;

			[TableColumnWidth(100, false)]
			[Button("폴더 열기")]
			[LabelText("폴더")]
			public void OepnFolder()
			{
				var path = 파일경로.Replace('/', '\\');
				System.Diagnostics.Process.Start("Explorer.exe", "/select," + path);
			}

			public LatestUpdateDBData(Type type, bool _isChoose, string _objectPath)
			{
				kType = type;
				선택 = _isChoose;
				파일경로 = _objectPath;
			}
		}

		[Serializable]
		public class OdinExcelData
		{
		
			[TableColumnWidth(30, false)] public bool 선택;
			[TableColumnWidth(650, false)] public string 엑셀위치;

			[TableColumnWidth(100, false)]
			[Button("파일 열기")]
			[LabelText("파일")]
			public void OpenFile()
			{
				System.Diagnostics.Process.Start(엑셀위치);
			}

			[TableColumnWidth(100, false)]
			[Button("폴더 열기")]
			[LabelText("폴더")]
			public void OepnFolder()
			{
				var path = 엑셀위치.Replace('/', '\\');
				System.Diagnostics.Process.Start("Explorer.exe", "/select," + path);
			}

			public OdinExcelData(bool isChoose, string excelPath)
			{
				선택 = isChoose;
				엑셀위치 = excelPath;
			}
		}
	}

	public class EasyExcelNoticeWindow : OdinEditorWindow
	{
		[TextArea(10, 15)] [LabelText("확인사항")] public string CheckStr;

		private Action applyAct;
		private Action cancelAct;

		public Action ApplyAct
		{
			get => applyAct;
			set => applyAct = value;
		}

		public Action CancelAct
		{
			get => cancelAct;
			set => cancelAct = value;
		}


		[Button("확인")]
		public void Proceed()
		{
			ApplyAct.Invoke();
		}

		[Button("취소")]
		public void Cancel()
		{
			CancelAct.Invoke();
		}
	}

	public class ResultWindow : OdinEditorWindow
	{
		[TextArea(10, 15)] [LabelText("확인사항")] public string CheckStr;


		[Button("확인")]
		public void Proceed()
		{
			Close();
		}
	}
}