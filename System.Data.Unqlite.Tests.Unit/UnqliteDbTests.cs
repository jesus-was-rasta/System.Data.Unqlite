#region Usings
using System.Text;

using NUnit.Framework;
#endregion


namespace System.Data.Unqlite.Tests.Unit
{
	[TestFixture, Category("Unit"), Description("UnqliteDb tests")]
	public class UnqliteDbTests
	{
		#region Costants
		private const string InMemoryDatabase = ":mem:";
		#endregion


		# region Setup and Tear Down
		/// <summary>
		/// SetsUp is called once before each Test within the same TestFxiture
		/// </summary>
		[SetUp]
		public void SetUp()
		{
			// Set up code here.
			// If this throws an exception no Test in the TestFixture are run.
		}

		/// <summary>
		/// TearsDown is called once after each Test within the same TestFixture.
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			// Clear up code here.
			// Will not run if no tess are run due to [SetUp] throwing an exception
		}
		# endregion


		#region Open tests
		[Test]
		public void UnqliteDb_Open_DoesNotThrowException()
		{
			Assert.DoesNotThrow(() =>
			{
				using (var unqliteDb = new UnqliteDb())
				{
					unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				}				
			});
		}
		#endregion

		[Test]
		public void UnqliteDb_KeyValue_Store()
		{
			const string testkey = "testKey";
			const string testValue = "testValue";

			string expectedValue = string.Empty;
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				unqliteDb.SaveKeyValue(testkey, testValue);
			
				expectedValue = unqliteDb.GetKeyValue(testkey);
			}

			Assert.IsTrue(expectedValue == testValue);
		}

		[Test]
		public void UnqliteDb_KeyValue_Store_With_Callback()
		{
			const string testkey = "testKey";
			const string testValue = "testValue";

			string expectedValue = string.Empty;
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				unqliteDb.SaveKeyValue(testkey, testValue);
				unqliteDb.GetKeyValue(testkey, value => expectedValue = value);
			}

			Assert.IsTrue(expectedValue == testValue);
		}

		[Test]
		public void Unqlite_KeyValue_BinaryStore_With_Callback()
		{
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				unqliteDb.SaveKeyValue("test", "hello world");
				unqliteDb.GetKeyBinaryValue("test", value =>
				{
					string strValue = Encoding.ASCII.GetString(value, 0, value.Length);
					Assert.IsTrue(strValue == "hello world");
				});
			}
		}

		[Test]
		public void Unqlite_KeyValue_Cursor()
		{
			using (var db = new UnqliteDb())
			{
				db.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				for (int i = 0; i < 100; i++)
				{
					db.SaveKeyValue("test" + (i + 1), "hello world " + (i + 1));
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
		}

		[Test]
		public void Unqlite_KeyValue_Cursor_with_callback()
		{
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				for (int i = 0; i < 20; i++)
				{
					unqliteDb.SaveKeyValue("test" + (i + 1), "hello world " + (i + 1));
				}
				using (var cursor = unqliteDb.CreateKeyValueCursor())
				{
					while (cursor.Read())
					{
						cursor.GetStringKey(key => Console.Out.WriteLine("Key:" + key));
						cursor.GetStringValue(value => Console.Out.WriteLine("Value:" + value));
						cursor.Next();
					}
				}
				unqliteDb.Close();
			}
		}

		[Test]
		public void Unqlite_KeyValue_Cursor_Seek()
		{
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				for (int i = 0; i < 20; i++)
				{
					unqliteDb.SaveKeyValue("test" + (i + 1), "hello world " + (i + 1));
				}
				using (var cursor = unqliteDb.CreateKeyValueCursor())
				{
					if (cursor.Read())
					{
						cursor.Seek("test1");
						string value = cursor.GetValue();
						Assert.IsTrue(value == "hello world 1");
					}
				}
				unqliteDb.Close();
			}
		}

		[Test]
		public void Unqlite_KeyValue_Cursor_SeekModeGE()
		{
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				for (int i = 0; i < 20; i++)
				{
					unqliteDb.SaveKeyValue("test" + (i + 1), "hello world " + (i + 1));
				}
				using (var cursor = unqliteDb.CreateKeyValueCursor())
				{
					if (cursor.Read())
					{
						cursor.Seek("test1", UnqliteCursorSeek.Match_GE);
						string value = cursor.GetValue();
						Assert.IsTrue(value == "hello world 1");
					}
				}
				unqliteDb.Close();
			}
		}

		[Test]
		public void Unqlite_KeyValue_Cursor_Seek_Delete()
		{
			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(InMemoryDatabase, UnqliteOpenMode.CREATE);
				for (int i = 0; i < 20; i++)
				{
					unqliteDb.SaveKeyValue("test" + (i + 1), "hello world " + (i + 1));
				}
				using (var cursor = unqliteDb.CreateKeyValueCursor())
				{
					if (cursor.Read())
					{
						cursor.Seek("test4", UnqliteCursorSeek.Match_GE);
						bool deleted = cursor.Delete();
						Assert.IsTrue(deleted);
					}
				}
				unqliteDb.Close();
			}
		}
    }
}
