using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


namespace EasyExcel
{
	/// <summary>
	///     Excel Converter
	/// Excel to CSharpFile
	/// </summary>
	public static partial class EEConverter
	{
		public static void GenerateCSharpFiles(string excelPath, string csPath, string[] selectedPath = null)
		{
			
				List<string> latestUpdateList = new List<string>();
				excelPath = excelPath.Replace("\\", "/");
				csPath = csPath.Replace("\\", "/");
				if (!csPath.EndsWith("/"))
					csPath += "/";

				var csChanged = false;

				//오딘설정파일 가져오기
				string[] filePaths = OdinEasyExcelMenuTree.LoadSelectPath();
				 var stoCreateList = new List<string>();

				List<string> classNameList = new List<string>();
				for (var i = 0; i < filePaths.Length; ++i)
				{
					var excelFilePath = filePaths[i].Replace("\\", "/");
					if (i + 1 < filePaths.Length)
						UpdateProgressBar(i + 1, filePaths.Length, "");
					else
						ClearProgressBar();
					if (!IsExcelFile(excelFilePath))
						continue;
					string fileName = Path.GetFileName(excelFilePath);
					var newCsDict = ToCSharpArray(excelFilePath);
					
					classNameList = newCsDict.Keys.ToList();
					foreach (var newCs in newCsDict)
					{
						var cSharpFileName = EESettings.Current.GetCSharpFileName(fileName, newCs.Key);
						var csFilePath = string.Empty;
						var dirPath = string.Empty;

						if (cSharpFileName.Contains("Server"))
						{
							csFilePath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath + cSharpFileName;
							dirPath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath;
							
						}

						if (cSharpFileName.Contains("Client"))
						{
							csFilePath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath + cSharpFileName;
							dirPath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath;
						
							cSharpFileName = cSharpFileName.Replace(".cs", string.Empty);
							
							latestUpdateList.Add(	RemoveOverwrite(cSharpFileName));
						}
						
						//본파일
						if (File.Exists(csFilePath))
						{
							File.Delete(csFilePath);
						}
						//메타파일
						if (File.Exists(csFilePath+".meta"))
						{
							File.Delete(csFilePath+".meta");
						}

						if (!Directory.Exists(dirPath))
						{
							Directory.CreateDirectory(dirPath);
						}

						File.WriteAllText(csFilePath, newCs.Value, Encoding.UTF8);
					}

					//서버 또는 클라 C# 스크립트 키 
					var serverStr = classNameList.Find(d => d.Contains("Server"));
					var clientStr = classNameList.Find(d => d.Contains("Client"));
					stoCreateList.Add(serverStr);
					stoCreateList.Add(clientStr);
					// //인스펙터생성
					// var newInspectorDict = ToCSharpInspectorArray(excelFilePath, serverStr, clientStr);
					//
					//
					// foreach (var newCs in newInspectorDict)
					// {
					// 	var inspectorFileName = EESettings.Current.GetSheetInspectorFileName(fileName, newCs.Key);
					// 	var csFilePath = string.Empty;
					// 	var dirPath = string.Empty;
					// 	if (inspectorFileName.Contains("Server"))
					// 	{
					// 		csFilePath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath + "Editor/" + inspectorFileName;
					// 		dirPath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveServerCsPath + "Editor/";
					// 	}
					//
					// 	if (inspectorFileName.Contains("Client"))
					// 	{
					// 		csFilePath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath + "Editor/" + inspectorFileName;
					// 		dirPath = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().SaveClientCsPath + "Editor/";
					// 	}
					// 	//본파일
					// 	if (File.Exists(csFilePath))
					// 	{
					// 		File.Delete(csFilePath);
					// 	}
					// 	//메타파일
					// 	if (File.Exists(csFilePath+".meta"))
					// 	{
					// 		File.Delete(csFilePath+".meta");
					// 	}
					// 	
					// 	if (!Directory.Exists(dirPath))
					// 	{
					// 		Directory.CreateDirectory(dirPath);
					// 	}
					// 	Directory.CreateDirectory(dirPath);
					//
					// 	File.WriteAllText(csFilePath, newCs.Value, Encoding.UTF8);
					//
					// 	
					// }
					
					
				} 
				SetCreateStoList(stoCreateList); 
				OdinEasyExcelMenuTree.SaveLatestCreateDB(latestUpdateList);
				//결과확인
				 bool isDone=EditorUtility.DisplayDialog("알림", "Table Class가 모두 업데이트 되었습니다.\n Json생성을 시작합니다", "확인");
				 if (isDone)
				 {
					 AssetDatabase.Refresh();
					 if (EditorUtility.DisplayCancelableProgressBar("Json생성", "Json생성대기중", 0))
					 {
						 File.Delete(Application.persistentDataPath + "/" + "CreateSTOList.json");
						 EditorUtility.ClearProgressBar();
					 }
					
				 }

			
			// catch (Exception e)
			// {
			// 	EELog.LogError(e.ToString());
			// 	OdinEasyExcelMenuTree.SaveLatestCreateDB(new List<string>());
			// 	EditorPrefs.SetBool(csChangedKey, false);
			// 	ClearProgressBar();
			// 	AssetDatabase.Refresh();
			// }
		}

		private static void SetCreateStoList(List<string> createSTOFileList)
		{
			var json = JsonConvert.SerializeObject(createSTOFileList);
			var file=File.CreateText(Application.persistentDataPath + "/" + "CreateSTOList.json");
			file.WriteLine(json);
			file.Close();
		}
		/// <summary> STO생성 </summary>
	
		private static Dictionary<string, string> ToCSharpArray(string excelPath)
		{
			var lst = new Dictionary<string, string>();
			var book = EEWorkbook.Load(excelPath);
			if (book == null)
				return lst;
			string fileName = Path.GetFileName(excelPath);
			foreach (var sheet in book.sheets)
			{
				if (sheet == null)
					continue;
				if (!IsValidSheet(sheet))
				{
					EELog.Log(string.Format("Skipped sheet {0} in file {1}.", sheet.name, fileName));
					continue;
				}

				var sheetData = ToSheetData(sheet);

				var csTxtServer = ToCSharpServer(sheetData, $"{sheet.name}_Server", fileName);
				var csTxtClient = ToCSharpClient(sheetData, $"{sheet.name}_Client", fileName);
				var cppTxtServer = CreateCppServerScript(sheetData, $"{sheet.name}_Server", fileName);
				//cpp생성
				if (OdinEasyExcelMenuTree.kTotalMenu.GetSetting().isCreateCpp)
				{
					var saveFileName = $"{sheet.name}_Server";
					SaveServerCppFile(saveFileName, cppTxtServer);
				}
				
				if (!string.IsNullOrEmpty(csTxtServer))
				{
					lst.Add($"{sheet.name}_Server", csTxtServer);
				}

				if (!string.IsNullOrEmpty(csTxtClient))
				{
					lst.Add($"{sheet.name}_Client", csTxtClient);
				}
			}

			return lst;
		}

		/// <summary>cpp파일 저장</summary>
		private static void SaveServerCppFile(string fileName, string cppStr)
		{
			//cpp생성
			var dir = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().GetSaveCppPath();
			string saveCppname = $"{fileName}.h";
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}

			var file=File.CreateText(dir + "/" + saveCppname);
			file.Close();
			StreamWriter sw = new StreamWriter(new FileStream(dir + "/" + saveCppname, FileMode.Open, FileAccess.ReadWrite), Encoding.UTF8);
			
			sw.WriteLine(cppStr);
			sw.Close();
			//줄끝정규화
			byte[] strToByte = File.ReadAllBytes(dir + "/" + saveCppname);
			for (int i = 0; i < strToByte.Length; i++)
			{
				if (strToByte[i] == 0x0D )
					strToByte[i] = 0x0A;
			}
			File.WriteAllBytes(dir + "/" + saveCppname, strToByte);
		}

		/// <summary>서버 Cpp String생성 </summary>
		private static string CreateCppServerScript(SheetData sheetData, string sheetName, string fileName)
		{
			bool keyFieldFound = false;
			var columnCount = sheetData.ColumnCount;
			List<EEColumnField> columnFields = new List<EEColumnField>();

			for (var col = 0; col < columnCount; col++)
			{
				//0=None 1=Client 2=Server 3=Both
				var dataTypeColumnCount = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataColumn;
				if (string.IsNullOrEmpty(sheetData.Table[dataTypeColumnCount][col]))
				{
#if LOG
					Log.Error($"해당 값은 비어있습니다! :{dataTypeColumnCount}:{col} ");
					return null;
#endif					
				}
				var intVal = int.TryParse(sheetData.Table[dataTypeColumnCount][col], out int value);
				if (intVal == false)
				{
#if LOG
					Log.Error($"해당 값은 int로 변경할 수 없습니다! : {sheetData.Table[dataTypeColumnCount][col]}");
					return null;
#endif
				}

				EnumDef.DBType dbtype = (EnumDef.DBType) value;
				if (dbtype == EnumDef.DBType.Client || dbtype == EnumDef.DBType.None) continue;
				//칼럼이름
				var rawColumnName = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataType, col);

				var rawColumnType = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kPropertyType, col);
				var ret = EEColumnFieldParser.Parse(col, rawColumnName, rawColumnType);
				columnFields.Add(ret);
				if (ret != null && ret.isKeyField)
					keyFieldFound = true;
			}

			//칼럼갯수 다시세팅
			columnCount = columnFields.Count;
			if (columnCount == 0)
			{
				return string.Empty;
			}

			if (keyFieldFound == false)
				EELog.LogError("Cannot find Key column in sheet " + sheetName);
			var csFile = new StringBuilder(2048);

			csFile.Append("#pragma once");
			csFile.Append("\n\n");
			csFile.Append("#include \"YMJsonData.h\"");
			csFile.Append("\n\n");
			string className = $"{sheetName}";
			csFile.Append($"class {className} : public YMJsonData");
			csFile.Append("\n");
			csFile.Append("{\n");
			csFile.Append("public:");
			csFile.Append("\n\n");
			csFile.Append("\t");
			csFile.Append("void Serialize(std::string Json)");

			csFile.Append("\n\t{"); 
			csFile.Append("\n");
			csFile.Append("\t\tJson::Value root;");
			csFile.Append("\n");
			csFile.Append("\t\tJsonParse(root, Json);");
			csFile.Append("\n\n");

			//파싱 함수
			for (var col = 0; col < columnCount; col++)
			{
				var columnField = columnFields[col];
				if (columnField == null) continue;
				//칼럼이름
				var rawColumnName = columnField.rawFiledName;
				if (columnField.isKeyField)
				{
					rawColumnName = columnField.rawFiledName.Split(':')[0];
				}

				//자료형
				var rawColumnType = columnField.rawFieldType;
				switch (rawColumnType)
				{
					case "long":
						csFile.Append($"\t\tYMJsonGetValue(root, \"{rawColumnName}\", {rawColumnName}, 0);");
						csFile.Append("\n");
						break;
					case "string":
						csFile.Append($"\t\tYMJsonGetValueString(root, \"{rawColumnName}\", {rawColumnName}, \"\");");
						csFile.Append("\n");
						
						break;
					case "float":
						csFile.Append($"\t\tYMJsonGetValue(root, \"{rawColumnName}\", {rawColumnName}, 0.f);");
						csFile.Append("\n");
						break;
					case "int":
						csFile.Append($"\t\tYMJsonGetValue(root, \"{rawColumnName}\", {rawColumnName}, 0);");
						csFile.Append("\n");
						break;
				}
			}

			csFile.Append("\t}");
			csFile.Append("\n\n");
			csFile.Append("public:");
			csFile.Append("\n\n");

			//선언
			for (var col = 0; col < columnCount; col++)
			{
				var columnField = columnFields[col];
				if (columnField == null)
					continue;
				//칼럼이름
				var rawColumnName = columnField.rawFiledName;
				if (columnField.isKeyField)
				{
					rawColumnName = columnField.rawFiledName.Split(':')[0];
				}

				//자료형
				var rawColumnType = columnField.rawFieldType;
				switch (rawColumnType)
				{
					case "string":
						rawColumnType = "std::string";
						break;
					case "long":
						rawColumnType = "Int64";
						break;
				}

				csFile.Append($"\t{rawColumnType} {rawColumnName};");
				csFile.Append("\n");
			}

			csFile.Append("};");
			
			

			return csFile.ToString();
		}

		private static string ToCSharpServer(SheetData sheetData, string sheetName, string fileName)
		{
			var rowDataClassName = $"{EESettings.Current.GetRowDataClassName(fileName, sheetName)}";

			var sheetClassName = EESettings.Current.GetSheetClassName(fileName, sheetName);
			var csFile = new StringBuilder(2048);
			csFile.Append("//------------------------------------------------------------------------------\n");
			csFile.Append("// <auto-generated>\n");
			csFile.Append("//     This code was generated by EasyExcel.\n");
			csFile.Append("//     Runtime Version: " + EEConstant.Version + "\n");
			csFile.Append("//\n");
			csFile.Append("//     Changes to this file may cause incorrect behavior and will be lost if\n");
			csFile.Append("//     the code is regenerated.\n");
			csFile.Append("// </auto-generated>\n");
			csFile.Append("//------------------------------------------------------------------------------");
			csFile.Append("\nusing System;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing EasyExcel;\n\n");
			csFile.Append(string.Format("namespace {0}\n", EESettings.Current.GetNameSpace(fileName)));
			csFile.Append("{\n");
			csFile.Append("\t[Serializable]\n");
			csFile.Append($"\tpublic class {rowDataClassName} : EERowData\n");
			csFile.Append("\t{\n");

			bool keyFieldFound = false;
			var columnCount = sheetData.ColumnCount;
			List<EEColumnField> columnFields = new List<EEColumnField>();

			for (var col = 0; col < columnCount; col++)
			{
				//0=None 1=Client 2=Server 3=Both
				var dataTypeColumnCount = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataColumn;
				if (string.IsNullOrEmpty(sheetData.Table[dataTypeColumnCount][col])) continue;
				
				EnumDef.DBType dbtype;
				dbtype = (EnumDef.DBType) int.Parse(sheetData.Table[dataTypeColumnCount][col]);
				
				if (dbtype == EnumDef.DBType.Client || dbtype == EnumDef.DBType.None) continue;
				//칼럼이름
				var rawColumnName = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataType, col);
				//자료형
				var rawColumnType = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kPropertyType, col);
				var ret = EEColumnFieldParser.Parse(col, rawColumnName, rawColumnType);
				columnFields.Add(ret);
				if (ret != null && ret.isKeyField)
					keyFieldFound = true;
			}

			//칼럼갯수 다시세팅
			columnCount = columnFields.Count;
			if (columnCount == 0)
			{
				return string.Empty;
			}

			if (keyFieldFound == false)
				EELog.LogError("Cannot find Key column in sheet " + sheetName);

			for (var col = 0; col < columnCount; col++)
			{
				var columnField = columnFields[col];
				if (columnField == null) continue;

				csFile.Append(columnField.GetDeclarationLines());
			}

			csFile.AppendFormat("\n\t\tpublic {0}()\n", rowDataClassName);
			csFile.Append("\t\t{\n");
			csFile.Append("\t\t}\n");
			csFile.Append("\n#if UNITY_EDITOR\n");
			csFile.AppendFormat("\t\tpublic {0}(List<List<string>> sheet, int row, int column)\n", rowDataClassName);
			csFile.Append("\t\t{\n");

			for (var col = 0; col < columnCount; col++)
			{
				var columnField = columnFields[col];
				if (columnField == null)
					continue;
				csFile.Append(columnField.GetParseLines());
			}

			csFile.Append("\t\t}\n#endif\n");

			csFile.Append("\t\tpublic override void OnAfterSerialized()\n");
			csFile.Append("\t\t{\n");
			for (var col = 0; col < columnCount; col++)
			{
				var columnField = columnFields[col];
				if (columnField == null)
					continue;
				csFile.Append(columnField.GetAfterSerializedLines());
			}

			csFile.Append("\t\t}\n");
			csFile.Append("\t}\n\n");

			// EERowDataCollection class
			csFile.Append("\tpublic class " + sheetClassName + " : EERowDataCollection\n");
			csFile.Append("\t{\n");
			csFile.AppendFormat("\t\t\n\t\tpublic List<{0}> elements = new List<{0}>();\n\n", rowDataClassName);

			csFile.AppendFormat("\t\tpublic override void AddData(EERowData data)\n\t\t{{\n\t\t\telements.Add(data as {0});\n\t\t}}\n\n", rowDataClassName);
			csFile.Append("\t\tpublic override int GetDataCount()\n\t\t{\n\t\t\treturn elements.Count;\n\t\t}\n\n");
			csFile.Append("\t\tpublic override EERowData GetData(int index)\n\t\t{\n\t\t\treturn elements[index];\n\t\t}\n\n");
			csFile.Append("\t\tpublic override void OnAfterSerialized()\n\t\t{\n");
			csFile.Append("\t\t\tforeach (var element in elements)\n");
			csFile.Append("\t\t\t\telement.OnAfterSerialized();\n");
			csFile.Append("\t\t}\n");

			csFile.Append("\t}\n");

			csFile.Append("}\n");

			return csFile.ToString();
		}

		private static string ToCSharpClient(SheetData sheetData, string sheetName, string fileName)
		{
			try
			{
				var rowDataClassName = $"{EESettings.Current.GetRowDataClassName(fileName, sheetName)}";
				var sheetClassName = EESettings.Current.GetSheetClassName(fileName, sheetName);
				var csFile = new StringBuilder(2048);
				csFile.Append("//------------------------------------------------------------------------------\n");
				csFile.Append("// <auto-generated>\n");
				csFile.Append("//     This code was generated by EasyExcel.\n");
				csFile.Append("//     Runtime Version: " + EEConstant.Version + "\n");
				csFile.Append("//\n");
				csFile.Append("//     Changes to this file may cause incorrect behavior and will be lost if\n");
				csFile.Append("//     the code is regenerated.\n");
				csFile.Append("// </auto-generated>\n");
				csFile.Append("//------------------------------------------------------------------------------");
				csFile.Append("\nusing System;\nusing System.Collections.Generic;\nusing UnityEngine;\nusing EasyExcel;\n\n");
				csFile.Append(string.Format("namespace {0}\n", EESettings.Current.GetNameSpace(fileName)));
				csFile.Append("{\n");
				csFile.Append("\t[Serializable]\n");
				csFile.Append($"\tpublic class {rowDataClassName} : EERowData\n");
				csFile.Append("\t{\n");

				bool keyFieldFound = false;
				var columnCount = sheetData.ColumnCount;
				List<EEColumnField> columnFields = new List<EEColumnField>();
				var dataTypeColumnCount = OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataColumn;

				for (var col = 0; col < columnCount; col++)
				{
					//0=Server 1=Client 2=Both
					if (string.IsNullOrEmpty(sheetData.Table[dataTypeColumnCount][col])) continue;
					EnumDef.DBType dbtype = (EnumDef.DBType) int.Parse(sheetData.Table[OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataColumn][col]);
					if (dbtype == EnumDef.DBType.Server || dbtype == EnumDef.DBType.None) continue;
					//칼럼이름
					var rawColumnName = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kDataType, col);

					var rawColumnType = sheetData.Get(OdinEasyExcelMenuTree.kTotalMenu.GetSetting().kPropertyType, col);
					var ret = EEColumnFieldParser.Parse(col, rawColumnName, rawColumnType);
					columnFields.Add(ret);
					if (ret != null && ret.isKeyField)
						keyFieldFound = true;
				}

				columnCount = columnFields.Count;
				if (!keyFieldFound)
					EELog.LogError("Cannot find Key column in sheet " + sheetName);

				for (var col = 0; col < columnCount; col++)
				{
					var columnField = columnFields[col];
					if (columnField == null) continue;

					csFile.Append(columnField.GetDeclarationLines());
				}

				csFile.AppendFormat("\n\t\tpublic {0}()\n", rowDataClassName);
				csFile.Append("\t\t{\n");
				csFile.Append("\t\t}\n");
				csFile.Append("\n#if UNITY_EDITOR\n");
				csFile.AppendFormat("\t\tpublic {0}(List<List<string>> sheet, int row, int column)\n", rowDataClassName);
				csFile.Append("\t\t{\n");
				columnCount = columnFields.Count;
				for (var col = 0; col < columnCount; col++)
				{
					var columnField = columnFields[col];
					if (columnField == null)
						continue;
					csFile.Append(columnField.GetParseLines());
				}

				csFile.Append("\t\t}\n#endif\n");

				csFile.Append("\t\tpublic override void OnAfterSerialized()\n");
				csFile.Append("\t\t{\n");
				for (var col = 0; col < columnCount; col++)
				{
					var columnField = columnFields[col];
					if (columnField == null)
						continue;
					csFile.Append(columnField.GetAfterSerializedLines());
				}

				csFile.Append("\t\t}\n");
				csFile.Append("\t}\n\n");

				// EERowDataCollection class
				csFile.Append("\tpublic class " + sheetClassName + " : EERowDataCollection\n");
				csFile.Append("\t{\n");
				csFile.AppendFormat("\t\t\n\t\tpublic List<{0}> elements = new List<{0}>();\n\n", rowDataClassName);

				csFile.AppendFormat("\t\tpublic override void AddData(EERowData data)\n\t\t{{\n\t\t\telements.Add(data as {0});\n\t\t}}\n\n", rowDataClassName);
				csFile.Append("\t\tpublic override int GetDataCount()\n\t\t{\n\t\t\treturn elements.Count;\n\t\t}\n\n");
				//elements가져오기
				csFile.AppendFormat("\t\npublic List<{0}> OnGetAllData()\n", rowDataClassName);
				csFile.Append("\t\t{");
				csFile.Append("\t\t\nreturn elements;\n");
				csFile.Append("\t\t}");

				csFile.Append("\t\tpublic override EERowData GetData(int index)\n\t\t{\n\t\t\treturn elements[index];\n\t\t}\n\n");
				csFile.Append("\t\tpublic override void OnAfterSerialized()\n\t\t{\n");
				csFile.Append("\t\t\tforeach (var element in elements)\n");
				csFile.Append("\t\t\t\telement.OnAfterSerialized();\n");


				csFile.Append("\t\t}\n");

				csFile.Append("\t}\n");

				csFile.Append("}\n");

				return csFile.ToString();
			}
			catch (Exception ex)
			{
				EELog.LogError(ex.ToString());
				OdinEasyExcelMenuTree.SaveLatestCreateDB(new List<string>());
			}

			return "";
		}

		private static Dictionary<string, string> ToCSharpInspectorArray(string excelPath, string serverCsharp, string clientCsharp)
		{
			var lst = new Dictionary<string, string>();
			var book = EEWorkbook.Load(excelPath);
			if (book == null)
				return lst;
			string fileName = Path.GetFileName(excelPath);
			foreach (var sheet in book.sheets)
			{
				if (sheet == null)
					continue;
				if (!IsValidSheet(sheet))
					continue;
				//서버 스크립트가 생성되었으면 만들기
				if (!string.IsNullOrEmpty(serverCsharp))
				{
					var csTxtServer = ToCSharpInspector($"{sheet.name}_Server", fileName);
					lst.Add($"{sheet.name}_Server", csTxtServer);
				}

				//클라 스크립트가 생성되었으면 만들기
				if (!string.IsNullOrEmpty(clientCsharp))
				{
					var csClient = ToCSharpInspector($"{sheet.name}_Client", fileName);

					lst.Add($"{sheet.name}_Client", csClient);
				}
			}

			return lst;
		}

		private static string ToCSharpInspector(string sheetName, string fileName)
		{
			try
			{
				var inspectorClassName = EESettings.Current.GetSheetInspectorClassName(fileName, sheetName);
				var csFile = new StringBuilder(1024);
				csFile.Append("//------------------------------------------------------------------------------\n");
				csFile.Append("// <auto-generated>\n");
				csFile.Append("//     This code was generated by EasyExcel.\n");
				csFile.Append("//     Runtime Version: " + EEConstant.Version + "\n");
				csFile.Append("//\n");
				csFile.Append("//     Changes to this file may cause incorrect behavior and will be lost if\n");
				csFile.Append("//     the code is regenerated.\n");
				csFile.Append("// </auto-generated>\n");
				csFile.Append("//------------------------------------------------------------------------------");

				csFile.Append("\nusing UnityEditor;\nusing EasyExcel;\n\n");
				csFile.Append(string.Format("namespace {0}\n", EESettings.Current.GetNameSpace(fileName)));
				csFile.Append("{\n");
				csFile.Append(string.Format("\t[CustomEditor(typeof({0}))]\n", EESettings.Current.GetSheetClassName(fileName, sheetName) /*sheetName, EESettings.Current.SheetDataPostfix*/));
				csFile.Append("\tpublic class " + inspectorClassName + " : EEAssetInspector\n");
				csFile.Append("\t{\n");
				csFile.Append("\t}\n");
				csFile.Append("}\n");

				return csFile.ToString();
			}
			catch (Exception ex)
			{
				EELog.LogError(ex.ToString());
				OdinEasyExcelMenuTree.SaveLatestCreateDB(new List<string>());
			}

			return "";
		}
	}
}