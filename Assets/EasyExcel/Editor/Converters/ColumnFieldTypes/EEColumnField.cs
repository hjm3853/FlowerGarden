
namespace EasyExcel
{
	/// <summary>
	/// Abstract Column field
	/// </summary>
	public abstract class EEColumnField
	{
		public  int rawColumnIndex;
		public  string rawFiledName;
		public  string rawFieldType;
		public  bool isKeyField;
		
		protected EEColumnField(int columnIndex, string rawColumnName, string rawColumnType)
		{
			rawColumnIndex = columnIndex;
			rawFiledName = rawColumnName;
			rawFieldType = rawColumnType;
			isKeyField = EEColumnFieldParser.IsKeyColumn(rawColumnName, rawColumnType);
		}
		
		public abstract string GetDeclarationLines();

		public abstract string GetParseLines();

		public virtual string GetAfterSerializedLines()
		{
			return null;
		}
	}

}