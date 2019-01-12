using System;
using System.Collections.Generic;

namespace EVEMon.Common.Models {
	/// <summary>
	/// Represents a unified wallet transaction. EVEMon 5 unifies the Wallet Journal and
	/// Wallet Transaction views into one, so some wallet transactions will be generated
	/// from Wallet Journal entries.
	/// </summary>
	public sealed class WalletTransaction : IHasID {
		/// <summary>
		/// The delta in wallet balance occurring from this transaction.
		/// </summary>
		public decimal Amount { get; }

		/// <summary>
		/// The wallet balance after this transaction.
		/// </summary>
		public decimal Balance { get; }

		/// <summary>
		/// An ID which has different meanings depending on the transaction type.
		/// </summary>
		public int ContextID { get; }

		/// <summary>
		/// The date when this transaction occurred.
		/// </summary>
		public DateTime Date { get; }

		/// <summary>
		/// The descriptive text of this transaction. Can be null or empty.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// The ID of the first party in this transaction. Has different meanings depending
		/// on the transaction type.
		/// </summary>
		public int FirstPartyID { get; }

		/// <summary>
		/// The wallet transaction ID. Can be zero for generated transactions (namely, sales
		/// to a buy order where escrow covers, since that generates a journal entry but no
		/// transaction).
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// The wallet journal entries associated with this transaction.
		/// </summary>
		public ICollection<WalletTransactionEntry> JournalEntries { get; }

		/// <summary>
		/// The reason given for this transaction. Can be null or empty.
		/// </summary>
		public string Reason { get; }

		/// <summary>
		/// The ID of the second party in this transaction. Has different meanings depending
		/// on the transaction type.
		/// </summary>
		public int SecondPartyID { get; }

		/// <summary>
		/// The amount of tax paid.	
		/// </summary>
		public decimal Tax { get; }

		/// <summary>
		/// The corporation who received the tax.
		/// </summary>
		public Corporation TaxReceiver { get; }

		/// <summary>
		/// The type of this transaction.
		/// </summary>
		public WalletTransactionType Type { get; }

		public override bool Equals(object obj) {
			// All transactions
			return obj is WalletTransaction other && other.ID == ID && (ID != 0L ||
				JournalEntries.Equals(other.JournalEntries));
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "{0:g} {1}: {2}".F(Date, Type, Description ?? "(none)");
		}
	}

	/// <summary>
	/// Entries which describe the individual item transactions that made up a wallet
	/// transaction in case of market orders.
	/// </summary>
	public sealed class WalletTransactionEntry : IHasID {
		/// <summary>
		/// The ID of the client. Could be a corporation or player.
		/// </summary>
		public int Client { get; }

		/// <summary>
		/// The date when the transaction occurred.
		/// </summary>
		public DateTime Date { get; }

		/// <summary>
		/// The wallet journal ID.
		/// </summary>
		public long ID { get; }

		/// <summary>
		/// Whether the order was a buy order (true) or sell order (false).
		/// </summary>
		public bool IsBuyOrder { get; }

		/// <summary>
		/// Whether the order was a personal order (true) or corp order (false).
		/// </summary>
		public bool IsPersonal { get; }

		/// <summary>
		/// The item transacted.
		/// </summary>
		public Item Item { get; }

		/// <summary>
		/// The location where the transaction occurred.
		/// </summary>
		public Structure Location { get; }

		/// <summary>
		/// The quantity transacted.
		/// </summary>
		public int Quantity { get; }

		/// <summary>
		/// The unit price of the item.
		/// </summary>
		public decimal UnitPrice { get; }

		public override bool Equals(object obj) {
			return obj is WalletTransactionEntry other && other.ID == ID;
		}

		public override int GetHashCode() {
			return ID.GetHashCode();
		}

		public override string ToString() {
			return "{0:g} {1:D}x{2}@{3:F2} {4}".F(Date, Quantity, Item, UnitPrice, IsBuyOrder ?
				"bought" : "sold");
		}
	}

	/// <summary>
	/// The valid types for wallet transactions.
	/// </summary>
	public enum WalletTransactionType {
		acceleration_gate_fee, advertisement_listing_fee, agent_donation,
		agent_location_services, agent_miscellaneous, agent_mission_collateral_paid,
		agent_mission_collateral_refunded, agent_mission_reward,
		agent_mission_reward_corporation_tax, agent_mission_time_bonus_reward,
		agent_mission_time_bonus_reward_corporation_tax, agent_security_services,
		agent_services_rendered, agents_preward, alliance_maintainance_fee,
		alliance_registration_fee, asset_safety_recovery_tax, bounty, bounty_prize,
		bounty_prize_corporation_tax, bounty_prizes, bounty_reimbursement, bounty_surcharge,
		brokers_fee, clone_activation, clone_transfer, contraband_fine, contract_auction_bid,
		contract_auction_bid_corp, contract_auction_bid_refund, contract_auction_sold,
		contract_brokers_fee, contract_brokers_fee_corp, contract_collateral,
		contract_collateral_deposited_corp, contract_collateral_payout,
		contract_collateral_refund, contract_deposit, contract_deposit_corp,
		contract_deposit_refund, contract_deposit_sales_tax, contract_price,
		contract_price_payment_corp, contract_reversal, contract_reward,
		contract_reward_deposited, contract_reward_deposited_corp, contract_reward_refund,
		contract_sales_tax, copying, corporate_reward_payout, corporate_reward_tax,
		corporation_account_withdrawal, corporation_bulk_payment, corporation_dividend_payment,
		corporation_liquidation, corporation_logo_change_cost, corporation_payment,
		corporation_registration_fee, courier_mission_escrow, cspa, cspaofflinerefund,
		datacore_fee, dna_modification_fee, docking_fee, duel_wager_escrow, duel_wager_payment,
		duel_wager_refund, factory_slot_rental_fee, gm_cash_transfer, industry_job_tax,
		infrastructure_hub_maintenance, inheritance, insurance, jump_clone_activation_fee,
		jump_clone_installation_fee, kill_right_fee, lp_store, manufacturing, market_escrow,
		market_fine_paid, market_transaction, medal_creation, medal_issued, mission_completion,
		mission_cost, mission_expiration, mission_reward, office_rental_fee, operation_bonus,
		opportunity_reward, planetary_construction, planetary_export_tax, planetary_import_tax,
		player_donation, player_trading, project_discovery_reward, project_discovery_tax,
		reaction, release_of_impounded_property, repair_bill, reprocessing_tax,
		researching_material_productivity, researching_technology,
		researching_time_productivity, resource_wars_reward, reverse_engineering,
		security_processing_fee, shares, sovereignity_bill, store_purchase,
		store_purchase_refund, transaction_tax, upkeep_adjustment_fee, war_ally_contract,
		war_fee, war_fee_surrender
	}
}
