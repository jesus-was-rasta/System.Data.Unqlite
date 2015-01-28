#region Usings
using System;
using System.Data.Unqlite;
using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;


#endregion


namespace UnitTesting
{
	[TestClass]
	public class Connection_Testing
	{
		private const string databaseName = ":mem:";

		[TestCleanup]
		public void Test_DeleteDB()
		{
			var dir = Path.GetDirectoryName(				Assembly.GetExecutingAssembly().Location);
			var dbFilePath = Path.Combine(dir, databaseName);
			if (File.Exists(dbFilePath))
			{
				File.Delete(dbFilePath);
			}
		}

		[TestMethod]
		public void Test_connection_Open_Create()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			if (res)
			{
				db.Close();
			}
			Assert.IsTrue(res);
		}

		[TestMethod]
		public void Test_KeyValue_Store()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			if (res)
			{
				var res1 = db.SaveKeyValue("test", "hello world");
				string value = db.GetKeyValue("test");
				Assert.IsTrue(value == "hello world");
				db.Close();
			}
		}

		[TestMethod]
		public void Test_KeyValue_Store_With_Callback()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			if (res)
			{
				var res1 = db.SaveKeyValue("test", "hello world");
				db.GetKeyValue("test", (value) => { Assert.IsTrue(value == "hello world"); });
				db.Close();
			}
		}

		[TestMethod]
		public void Test_KeyValue_BinaryStore_With_Callback()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			if (res)
			{
				var res1 = db.SaveKeyValue("test", "hello world");
				db.GetKeyBinaryValue("test", (value) =>
				{
					string strValue = Encoding.ASCII.GetString(value, 0, value.Length);
					Assert.IsTrue(strValue == "hello world");
				});
				db.Close();
			}
		}

		[TestMethod]
		public void Test_KeyValue_Cursor()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			for (int i = 0; i < 100; i++)
			{
				db.SaveKeyValue("test" + (i + 1).ToString(), "hello world " + (i + 1).ToString());
			}
			using (var cursor = db.CreateKeyValueCursor())
			{
				while (cursor.Read())
				{
					string key = cursor.GetKey();
					byte[] binaryKey = cursor.GetBinaryKey();
					Console.Out.WriteLine("Key:" + key);
					string value = cursor.GetValue();
					byte[] binaryValue = cursor.GetBinaryValue();
					Console.Out.WriteLine("Value:" + value);
					cursor.Next();
				}
			}
			db.Close();
		}

		[TestMethod]
		public void Test_KeyValue_Cursor_with_callback()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			for (int i = 0; i < 20; i++)
			{
				db.SaveKeyValue("test" + (i + 1).ToString(), "hello world " + (i + 1).ToString());
			}
			using (var cursor = db.CreateKeyValueCursor())
			{
				while (cursor.Read())
				{
					cursor.GetStringKey((key) => { Console.Out.WriteLine("Key:" + key); });
					cursor.GetStringValue((value) => { Console.Out.WriteLine("Value:" + value); });
					cursor.Next();
				}
			}
			db.Close();
		}

		[TestMethod]
		public void Test_KeyValue_Cursor_Seek()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			for (int i = 0; i < 20; i++)
			{
				db.SaveKeyValue("test" + (i + 1).ToString(), "hello world " + (i + 1).ToString());
			}
			using (var cursor = db.CreateKeyValueCursor())
			{
				if (cursor.Read())
				{
					cursor.Seek("test1");
					string value = cursor.GetValue();
					Assert.IsTrue(value == "hello world 1");
				}
			}
			db.Close();
		}

		[TestMethod]
		public void Test_KeyValue_Cursor_SeekModeGE()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			for (int i = 0; i < 20; i++)
			{
				db.SaveKeyValue("test" + (i + 1).ToString(), "hello world " + (i + 1).ToString());
			}
			using (var cursor = db.CreateKeyValueCursor())
			{
				if (cursor.Read())
				{
					cursor.Seek("test1", UnqliteCursorSeek.Match_GE);
					string value = cursor.GetValue();
					Assert.IsTrue(value == "hello world 1");
				}
			}
			db.Close();
		}

		[TestMethod]
		public void Test_KeyValue_Cursor_Seek_Delete()
		{
			UnqliteDb db = UnqliteDb.Create();
			var res = db.Open(databaseName, UnqliteOpenMode.CREATE);
			for (int i = 0; i < 20; i++)
			{
				db.SaveKeyValue("test" + (i + 1).ToString(), "hello world " + (i + 1).ToString());
			}
			using (var cursor = db.CreateKeyValueCursor())
			{
				if (cursor.Read())
				{
					cursor.Seek("test4", UnqliteCursorSeek.Match_GE);
					bool deleted = cursor.Delete();
					Assert.IsTrue(deleted);
				}
			}
			db.Close();
		}
	}
}