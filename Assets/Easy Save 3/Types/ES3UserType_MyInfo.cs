using System;
using UnityEngine;

namespace ES3Types
{
/*
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("local", "gold", "occupation", "freeDepositGold", "monthPurchasePrice", "extraInterestRate", "curYear", "curMonth", "curDay", "curHour", "curMin", "mCdProductList", "mLoanConditionList", "mInvenItemInfoList")]
	public class ES3UserType_MyInfo : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_MyInfo() : base(typeof(ClassDef.MyInfo)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (ClassDef.MyInfo)obj;
			
			writer.WriteProperty("local", instance.local, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(EnumDef.CityType)));
			writer.WriteProperty("gold", instance.gold, ES3Type_long.Instance);
			writer.WriteProperty("occupation", instance.occupation, ES3Type_float.Instance);
			writer.WriteProperty("freeDepositGold", instance.freeDepositGold, ES3Type_long.Instance);
			writer.WriteProperty("monthPurchasePrice", instance.monthPurchasePrice, ES3Type_long.Instance);
			writer.WriteProperty("extraInterestRate", instance.extraInterestRate, ES3Type_float.Instance);
			writer.WriteProperty("curYear", instance.curYear, ES3Type_int.Instance);
			writer.WriteProperty("curMonth", instance.curMonth, ES3Type_int.Instance);
			writer.WriteProperty("curDay", instance.curDay, ES3Type_int.Instance);
			writer.WriteProperty("curHour", instance.curHour, ES3Type_int.Instance);
			writer.WriteProperty("curMin", instance.curMin, ES3Type_int.Instance);
			writer.WritePrivateField("mCdProductList", instance);
			writer.WritePrivateField("mLoanConditionList", instance);
			writer.WritePrivateField("mInvenItemInfoList", instance);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ClassDef.MyInfo)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "local":
						instance.local = reader.Read<EnumDef.CityType>();
						break;
					case "gold":
						instance.gold = reader.Read<System.Int64>(ES3Type_long.Instance);
						break;
					case "occupation":
						instance.occupation = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "freeDepositGold":
						instance.freeDepositGold = reader.Read<System.Int64>(ES3Type_long.Instance);
						break;
					case "monthPurchasePrice":
						instance.monthPurchasePrice = reader.Read<System.Int64>(ES3Type_long.Instance);
						break;
					case "extraInterestRate":
						instance.extraInterestRate = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "curYear":
						instance.curYear = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "curMonth":
						instance.curMonth = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "curDay":
						instance.curDay = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "curHour":
						instance.curHour = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "curMin":
						instance.curMin = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "mCdProductList":
					instance = (ClassDef.MyInfo)reader.SetPrivateField("mCdProductList", reader.Read<System.Collections.Generic.List<ClassDef.CDProductInfo>>(), instance);
					break;
					case "mLoanConditionList":
					instance = (ClassDef.MyInfo)reader.SetPrivateField("mLoanConditionList", reader.Read<System.Collections.Generic.List<ClassDef.LoanCondition>>(), instance);
					break;
					case "mInvenItemInfoList":
					instance = (ClassDef.MyInfo)reader.SetPrivateField("mInvenItemInfoList", reader.Read<System.Collections.Generic.List<ClassDef.InvenItemInfo>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new ClassDef.MyInfo();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_MyInfoArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_MyInfoArray() : base(typeof(ClassDef.MyInfo[]), ES3UserType_MyInfo.Instance)
		{
			Instance = this;
		}
	}
*/
}