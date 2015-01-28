#region Usings
using System.Data.Unqlite.Interop;


#endregion


namespace System.Data.Unqlite
{
	public class UnqliteDb
	{
		#region Fields
		private readonly UnqliteDbProxy _dbProxy;
		#endregion


		#region Constructors
		internal UnqliteDb(UnqliteDbProxy proxy)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			_dbProxy = proxy;
		}
		#endregion


		#region Public Methods
		public static UnqliteDb Create()
		{
			var proxy = new UnqliteDbProxy();
			return new UnqliteDb(proxy);
		}

		public bool Open(string fileName, UnqliteOpenMode openMode)
		{
			return _dbProxy.Open(fileName, openMode);
		}

		public bool SaveKeyValue(string key, string value)
		{
			return _dbProxy.SaveKeyValue(key, value);
		}

		public bool SaveKeyBinaryValue(string key, byte[] value)
		{
			return _dbProxy.SaveKeyValue(key, value);
		}

		public string GetKeyValue(string key)
		{
			return _dbProxy.GetKeyValue(key);
		}

		public byte[] GetKeyBinaryValue(string key)
		{
			return _dbProxy.GetKeyBinaryValue(key);
		}

		public void GetKeyValue(string key, Action<string> action)
		{
			_dbProxy.GetKeyValue(key, action);
		}

		public void GetKeyBinaryValue(string key, Action<byte[]> action)
		{
			_dbProxy.GetKeyBinaryValue(key, action);
		}

		public void Close()
		{
			_dbProxy.Close();
		}

		public KeyValueCursor CreateKeyValueCursor()
		{
			return new KeyValueCursor(_dbProxy, true);
		}

		public KeyValueCursor CreateReverseKeyValueCursor()
		{
			return new KeyValueCursor(_dbProxy, false);
		}
		#endregion
	}
}