using System;
using UnityEngine;

namespace EasyExcel
{
	public interface IEEDataLoader
	{
		EERowDataCollection Load(string sheetClassName);
	}
	
	/// <summary>
	/// Load generated data by Resources.Load.
	/// </summary>
	public class EEDataLoaderResources : IEEDataLoader
	{
		public EERowDataCollection Load(string sheetClassName)
		{
			var path = EESettings.Current.GeneratedClientAssetPath.Replace("Assets/", "");
			var headName = sheetClassName;
			var filePath =Application.dataPath+"/"+ path + headName;
			//var table = Resources.Load(filePath) as EERowDataCollection;
			var table = Resources.Load("Data/" + headName+".asset")as EERowDataCollection;
			return table;
		}
	}
}