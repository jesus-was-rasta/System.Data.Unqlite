#region Usings
using System.Data.Unqlite.Interop;


#endregion


namespace System.Data.Unqlite
{
	public class UnqliteDb : IDisposable
	{
		#region Fields
		private UnqliteDbProxy _dbProxy;
		private bool _disposed;
		#endregion


		#region Constructors
		public UnqliteDb()
		{
		}
		#endregion


		#region Public Methods

		public bool Open(string fileName, UnqliteOpenMode openMode)
		{
			if (_dbProxy == null)
			{
				_dbProxy = new UnqliteDbProxy();	
			}
			return _dbProxy.Open(fileName, openMode);
		}

		public bool SaveKeyValue(string key, string value)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return _dbProxy.SaveKeyValue(key, value);
		}

		public bool SaveKeyBinaryValue(string key, byte[] value)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return _dbProxy.SaveKeyValue(key, value);
		}

		public string GetKeyValue(string key)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return _dbProxy.GetKeyValue(key);
		}

		public byte[] GetKeyBinaryValue(string key)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return _dbProxy.GetKeyBinaryValue(key);
		}

		public void GetKeyValue(string key, Action<string> action)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			_dbProxy.GetKeyValue(key, action);
		}

		public void GetKeyBinaryValue(string key, Action<byte[]> action)
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			_dbProxy.GetKeyBinaryValue(key, action);
		}

		public void Close()
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			_dbProxy.Close();
		}

		public KeyValueCursor CreateKeyValueCursor()
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return new KeyValueCursor(_dbProxy, true);
		}

		public KeyValueCursor CreateReverseKeyValueCursor()
		{
			if (_dbProxy == null)
			{
				throw new UnqliteDbNotOpenedException();
			}
			return new KeyValueCursor(_dbProxy, false);
		}
		#endregion


		#region Dispose

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				_dbProxy.Close();
				_dbProxy.Dispose();
			}

			_disposed = true;
		}

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~UnqliteDb()
		{
			Dispose(false);
		}
		#endregion
	}
}