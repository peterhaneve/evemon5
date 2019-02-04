using EVEMon.Common.Abstractions;
using EVEMon.Common.Esi;
using EVEMon.Common.Esi.RequestObjects;
using EVEMon.Common.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace EVEMon.Tests {
	/// <summary>
	/// Runs some simple tests for ESI requests.
	/// </summary>
	[TestClass]
	public class EsiRequestTests {
		private readonly IEveMonClient mockClient;

		public EsiRequestTests() {
			mockClient = new MockEveMonClient();
			mockClient.RequestHandler.ConfigureServicePoints();
		}

		[TestMethod]
		public void TestIDToName() {
			using (var lookup = new EntityLookupService(mockClient)) {
				lookup.Initialize();
				var a0 = lookup.GetAlliance(0);
				Assert.IsNotNull(a0);
				Assert.AreEqual(0, a0.ID);
				Assert.AreEqual(Constants.UNKNOWN_TEXT, a0.Name);
				var c0 = lookup.GetCharacter(0);
				Assert.IsNotNull(c0);
				Assert.AreEqual(0, c0.ID);
				Assert.AreEqual(0, c0.CorporationID);
				Assert.AreEqual(Constants.UNKNOWN_TEXT, c0.Name);
				var testComplete = new AutoResetEvent(false);
				var handler = new EventHandler((sender, args) => {
					var testAlliance = lookup.GetAlliance(498125261);
					var peterHan = lookup.GetCharacter(96325318);
					Assert.IsNotNull(testAlliance);
					Assert.IsNotNull(peterHan);
					if (peterHan.Name == "Peter Han" && testAlliance.Name ==
							"Test Alliance Please Ignore" && peterHan.AllianceID ==
							testAlliance.ID)
						testComplete.Set();
				});
				mockClient.Events.OnIDToName += handler;
				// Start these up
				Assert.IsNotNull(lookup.GetAlliance(498125261));
				Assert.IsNotNull(lookup.GetCharacter(96325318));
				Assert.IsTrue(testComplete.WaitOne(2000));
				mockClient.Events.OnIDToName -= handler;
			}
		}

		[TestMethod]
		public void TestPublicContracts() {
			TestPublicContractsAsync().Wait();
		}

		private async Task TestPublicContractsAsync() {
			var handler = mockClient.RequestHandler;
			var headers = new EsiRequestHeaders(EsiEndpoints.ContractsPublic) {
				Path = "10000002"
			};
			var result = await handler.QueryEsiGetAsync<Collection<
				Get_contracts_public_region_id_200_ok>>(headers);
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Status, EsiResultStatus.OK);
			Assert.IsNotNull(result.Result);
			Assert.AreNotEqual(0, result.Result.Count);
		}
	}
}
