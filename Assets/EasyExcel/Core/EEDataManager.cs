using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace EasyExcel
{
	using RowDataDictInt = Dictionary<int, EERowData>;
	using RowDataDictStr = Dictionary<string, EERowData>;
	/// <summary>
	/// UID:key= int->long으로 변경
	/// </summary>
	public class EEDataManager
	{
		private readonly IEEDataLoader dataLoader;
		private readonly Dictionary<Type, Dictionary<long, EERowData>> dataCollectionDictInt = new Dictionary<Type, Dictionary<long, EERowData>>();
		private readonly Dictionary<Type, RowDataDictStr> dataCollectionDictStr = new Dictionary<Type, RowDataDictStr>();
		
		public EEDataManager(IEEDataLoader _dataLoader = null)
		{
			dataLoader = _dataLoader ?? new EEDataLoaderResources();
		}
		
		#region Find with key
		
		public T Get<T>(long key) where T : EERowData
		{
			return Get(key, typeof(T)) as T;
		}
		
		public T Get<T>(string key) where T : EERowData
		{
			return Get(key, typeof(T)) as T;
		}

		public EERowData Get(long key, Type type)
		{
			Dictionary<long, EERowData> soDic;
			dataCollectionDictInt.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(key, out data);
			return data;
		}
		
		public EERowData Get(string key, Type type)
		{
			RowDataDictStr soDic;
			dataCollectionDictStr.TryGetValue(type, out soDic);
			if (soDic == null) return null;
			EERowData data;
			soDic.TryGetValue(key, out data);
			return data;
		}

		public List<T> GetListJson<T>()where T:EERowData
		{
			var type = typeof(T);
			//var a= Resources.Load<TextAsset>("Data/" + type.Name + ".json");
			var a = Resources.Load<TextAsset>("SheetData/" + type.Name);
			if (a == default)
			{
#if LOG
				Log.Error($"<GetListJson> :번들의  Data/{type.Name}.json 파일을 찾을 수 없습니다.\nEx) Excel파일의 이름과 SheetName이 동일하지 않습니다.");
#endif				
				return default;
			}
			JsonSerializerSettings _jsonWriter = new JsonSerializerSettings() {
				NullValueHandling = NullValueHandling.Ignore
			};
			var list = JsonConvert.DeserializeObject<List<T>>(a.text,_jsonWriter);
			return list;
		}
		
		public List<T> GetList<T>() where T : EERowData
		{
			Dictionary<long, EERowData> dictInt;
			dataCollectionDictInt.TryGetValue(typeof(T), out dictInt);
			if (dictInt != null)
			{
				var list = new List<T>();
				foreach (var data in dictInt)
					list.Add((T) data.Value);
				return list;
			}
			RowDataDictStr dictStr;
			dataCollectionDictStr.TryGetValue(typeof(T), out dictStr);
			if (dictStr != null)
			{
				var list = new List<T>();
				foreach (var data in dictStr)
					list.Add((T) data.Value);
				return list;
			}
			return null;
		}

		public List<EERowData> GetList(Type type)
		{
			Dictionary<long, EERowData> dictInt;
			dataCollectionDictInt.TryGetValue(type, out dictInt);
			if (dictInt != null)
				return dictInt.Values.ToList();
			RowDataDictStr dictStr;
			dataCollectionDictStr.TryGetValue(type, out dictStr);
			if (dictStr != null)
				return dictStr.Values.ToList();
			return null;
		}
		
		#endregion

		#region Load Assets
		
		public void Load()
		{
// #if UNITY_EDITOR
// 			if (!EESettings.Current.GeneratedClientAssetPath.Contains("/Resources/"))
// 			{
// 				UnityEditor.EditorUtility.DisplayDialog("EasyExcel",
// 					string.Format(
// 						"AssetPath of EasyExcel Settings MUST be in Resources folder.\nCurrent is {0}.",
// 						EESettings.Current.GeneratedClientAssetPath), "OK");
// 				return;
// 			}
// #endif
			dataCollectionDictInt.Clear();
			dataCollectionDictStr.Clear();

			var assembly = EEUtility.GetSourceAssembly();
			var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(EERowDataCollection)));
			foreach (var dataCollectionType in types)
			{
				if(dataCollectionType.FullName.Contains("Server"))continue;
				ParseOneDataCollection(assembly, dataCollectionType);
					
			}
			
			
			EELog.Log(string.Format("{0} tables loaded.", dataCollectionDictInt.Count + dataCollectionDictStr.Count));
		}

		private void ParseOneDataCollection(Assembly assembly, Type dataCollectionType)
		{
			var sheetClassName = "";
			try
			{
				sheetClassName = dataCollectionType.Name;//GetSheetName(dataCollectionType);
				var collection = dataLoader.Load(sheetClassName);
				if (collection == null&&sheetClassName.Contains("_Client"))
				{
					EELog.LogError("EEDataManager: Load asset error, sheet name " + sheetClassName);
					return;
				}

				if (sheetClassName.Contains("_Client"))
				{
					collection.OnAfterSerialized();
					var rowDataType = GetRowDataClassType(assembly, collection.ExcelFileName, dataCollectionType);
					var keyField = EEUtility.GetRowDataKeyField(rowDataType);
					if (keyField == null)
					{
						EELog.LogError("EEDataManager: Cannot find Key field in sheet " + sheetClassName);
						return;
					}

					var keyType = keyField.FieldType;
					//키타입 롱 2022.07.06장군왕
					if (keyType == typeof(long))
					{
						var dataDict = new Dictionary<long, EERowData>();
						for (var i = 0; i < collection.GetDataCount(); ++i)
						{
							var data = collection.GetData(i);
							long key = (long) keyField.GetValue(data);
							dataDict.Add(key, data);
						}
					
						dataCollectionDictInt.Add(rowDataType, dataDict);
					}
					else if (keyType == typeof(string))
					{
						var dataDict = new RowDataDictStr();
						for (var i = 0; i < collection.GetDataCount(); ++i)
						{
							var data = collection.GetData(i);
							string key = (string) keyField.GetValue(data);
							dataDict.Add(key, data);
						}

						dataCollectionDictStr.Add(rowDataType, dataDict);
					}
					else
					{
						EELog.LogError("sheetClassName" + sheetClassName + " : " + string.Format("Load {0} failed. There is no valid Key field in ", dataCollectionType.Name));
					}
				}
			
			}
			catch (Exception e)
			{
				EELog.LogError("sheetClassName" + sheetClassName + " : " + e.ToString());
			}
		}

		private static Type GetRowDataClassType(Assembly assembly, string excelFileName, Type sheetClassType)
		{
			var excelName = Path.GetFileNameWithoutExtension(excelFileName);
			var sheetName = GetSheetName(sheetClassType);
			var type = assembly.GetType(EESettings.Current.GetRowDataClassName(excelName, sheetName, true));
			return type;
		}

		private static string GetSheetName(Type sheetClassType)
		{
			return EESettings.Current.GetSheetName(sheetClassType);
		}

		#endregion
	}
}