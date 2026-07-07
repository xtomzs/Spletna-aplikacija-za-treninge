<script lang="ts">
	import Modal from "../../sharedComponents/Modal.svelte";
	import type { GraphLabel } from "./models/GraphLabel";

	interface Props {
		showModal?: boolean;
		weekData?: GraphLabel;
	}

	let { showModal = $bindable(), weekData } = $props();
</script>

<Modal bind:showModal>
	{#snippet header()}
		<h2 class="mb-0">Week Stats: {weekData?.Week || "Loading..."}</h2>
	{/snippet}

	<div class="week-stats-container">
		<div class="stats-grid">
			<div class="stat-card">
				<div class="stat-label">Total Distance</div>
				<div class="stat-value">{weekData?.Distance || "N/A"}</div>
			</div>
			<div class="stat-card">
				<div class="stat-label">Total Elevation Gain</div>
				<div class="stat-value">{weekData?.TotalElevationGain || "N/A"}</div>
			</div>
		</div>
		<button class="btn btn-secondary mt-4" onclick={() => (showModal = false)}>
			Close
		</button>
	</div>
</Modal>

<style>
	.week-stats-container {
		padding: 1rem 0;
	}

	.stats-grid {
		display: grid;
		grid-template-columns: 1fr 1fr;
		gap: 1.5rem;
		margin-bottom: 1rem;
	}

	.stat-card {
		padding: 1rem;
		background: #f8f9fa;
		border-radius: 0.5rem;
		border: 1px solid #dee2e6;
		text-align: center;
	}

	.stat-label {
		font-size: 0.875rem;
		color: #6c757d;
		margin-bottom: 0.5rem;
		font-weight: 500;
	}

	.stat-value {
		font-size: 1.5rem;
		font-weight: bold;
		color: #212529;
	}
</style>
