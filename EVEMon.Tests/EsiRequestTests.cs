using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EVEMon.Common.Esi;
using EVEMon.Common.Esi.RequestObjects;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EVEMon.Tests {
	/// <summary>
	/// Runs some simple tests for ESI requests.
	/// </summary>
	[TestClass]
	public class EsiRequestTests {
		[TestMethod]
		public void TestPublicContracts() {
			TestPublicContractsAsync().Wait();
		}

		private async Task TestPublicContractsAsync() {
			using (var handler = new EsiRequestHandler()) {
				handler.ConfigureServicePoints();
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
}
