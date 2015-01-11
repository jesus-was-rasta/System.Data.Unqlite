using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;


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
			UnqliteDb db = UnqliteDb.Create();

			var result = db.Open(InMemoryDatabase, UnqliteOpen.CREATE);
			if (result)
			{
				db.Close();
			}
			Assert.IsTrue(result);
		}
		#endregion
    }
}
