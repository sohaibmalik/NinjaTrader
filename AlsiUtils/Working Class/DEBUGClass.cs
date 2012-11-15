using System;


namespace AlsiUtils
{
	public  class DebugClass
	{
	public static string SenderApplication;

		public DebugClass(string SenderApp)
		{
			SenderApplication = SenderApp;
		}

		public static  void WriteLine(dynamic Debug)
		{
			DataBase.InsertDebugLog(DateTime.Now, Debug.ToString(),SenderApplication);
		}
	}

}
