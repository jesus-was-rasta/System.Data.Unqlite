#region Usings
using System.Data.Unqlite.Interop;
using System.Text;


#endregion


namespace System.Data.Unqlite
{
	public class KeyValueCursor : IDisposable
	{
		#region Fields
		private readonly UnqliteDbProxy _dbProxy;
		private IntPtr _cursor;
		#endregion


		#region Properties
		public bool Open { get; set; }
		#endregion


		#region Constructors
		internal KeyValueCursor(UnqliteDbProxy dbProxy, bool forwardCursor)
		{
			if (dbProxy == null)
			{
				throw new ArgumentNullException("dbProxy");
			}

			_dbProxy = dbProxy;
			bool success = dbProxy.InitKVCursor(out _cursor);
			if (success)
			{
				success = forwardCursor ? dbProxy.KVMoveToFirstEntry(_cursor) : dbProxy.KVMoveToLastEntry(_cursor);
				Open = true;
			}
			else
			{
				Open = false;
			}
		}
		#endregion


		#region Public Methods
		public bool Read()
		{
			return _dbProxy.KV_ValidEntry(_cursor);
		}

		public void Prev()
		{
			_dbProxy.KV_PrevEntry(_cursor);
		}

		public void Next()
		{
			_dbProxy.KV_NextEntry(_cursor);
		}

		public string GetKey()
		{
			byte[] keyData = _dbProxy.KV_GetCurrentKey(_cursor);
			return Encoding.ASCII.GetString(keyData);
		}

		public byte[] GetBinaryKey()
		{
			return _dbProxy.KV_GetCurrentKey(_cursor);
		}

		public string GetValue()
		{
			byte[] valueData = _dbProxy.KV_GetCurrentValue(_cursor);
			return Encoding.ASCII.GetString(valueData);
		}

		public byte[] GetBinaryValue()
		{
			return _dbProxy.KV_GetCurrentValue(_cursor);
		}

		public void GetStringKey(Action<string> action)
		{
			_dbProxy.GetCursorKeyValue(_cursor, action);
		}

		public void GetBinaryKey(Action<byte[]> action)
		{
			_dbProxy.GetCursorKeyValue(_cursor, action);
		}

		public void GetStringValue(Action<string> action)
		{
			_dbProxy.GetCursorValue(_cursor, action);
		}

		public void GetBinaryValue(Action<byte[]> action)
		{
			_dbProxy.GetCursorValue(_cursor, action);
		}

		public void Seek(string key)
		{
			_dbProxy.SeekKey(_cursor, key, UnqliteCursorSeek.Match_Exact);
		}

		public void Seek(string key, UnqliteCursorSeek seekMode)
		{
			_dbProxy.SeekKey(_cursor, key, seekMode);
		}

		public bool Delete()
		{
			return _dbProxy.DeleteEntry(_cursor);
		}
		#endregion


		#region Dispose
		public void Dispose()
		{
			_dbProxy.ReleaseCursor(_cursor);
			_cursor = IntPtr.Zero;
			Open = false;
		}
		#endregion

	}
}