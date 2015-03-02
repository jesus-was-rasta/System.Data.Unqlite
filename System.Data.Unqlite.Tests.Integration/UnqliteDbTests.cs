#region Usings
using System.IO;

using NUnit.Framework;


#endregion


namespace System.Data.Unqlite.Tests.Integration
{
	[TestFixture, Category("Integration"), Description("UnqliteDb tests")]
	public class UnqliteDbTests
	{
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


		#region Key Value Store
		[Test, MaxTime(100)]
		public void UnqliteDb_KeyValue_Store_1000_Items()
		{
			const int itemsCount = 1000;
			var databaseFile = Path.GetTempFileName();

			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(databaseFile, UnqliteOpenMode.CREATE);

				for (int i = 0; i < itemsCount; i++)
				{
					unqliteDb.SaveKeyValue(i.ToString(), i.ToString());
				}
			}
		}

		[Test, MaxTime(10000)]
		public void UnqliteDb_KeyValue_Store_1000000_Items()
		{
			const int itemsCount = 1000000;
			var databaseFile = Path.GetTempFileName();

			using (var unqliteDb = new UnqliteDb())
			{
				unqliteDb.Open(databaseFile, UnqliteOpenMode.CREATE);

				for (int i = 0; i < itemsCount; i++)
				{
					unqliteDb.SaveKeyValue(i.ToString(), i.ToString());
				}
			}
		}
		#endregion
	}
}
