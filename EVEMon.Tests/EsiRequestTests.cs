using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EVEMon.Common;
using EVEMon.Common.Esi;
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
			await Task.Delay(5000);
			using (var handler = new EsiRequestHandler()) {
				handler.ConfigureServicePoints();
				var result = await handler.QueryEsiGetAsync<Collection<
					Get_contracts_public_region_id_200_ok>>("https://" + Constants.ESI_BASE +
					"/v1/contracts/public/10000002/");
				Assert.IsNotNull(result);
				Assert.Equals(result.Status, EsiResultStatus.OK);
				Assert.IsNotNull(result.Result);
			}
		}
	}
}
