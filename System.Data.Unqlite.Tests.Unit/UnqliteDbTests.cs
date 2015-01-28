#region Usings
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
    }
}
