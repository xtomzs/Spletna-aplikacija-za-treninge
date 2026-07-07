<script lang="ts">
	import { formatDistance } from "../../Utilities/DataFormat";
	import { Cell } from "./models/Cell.svelte";
	import type { PlannerItem } from "./models/PlannerItem";
    import { PlannerItemModalModel } from "./models/PlannerItemModal.Models.svelte";
	import type { RealizationItem } from "./models/RealizationItem";
	import PlannerItemModal from "./PlannerItemModal.svelte";

	let modalProps = $state(new PlannerItemModalModel(false, "", null));
	let month: number = new Date().getMonth(); // 0–11
	let year: number = new Date().getFullYear();	
	let loading: boolean = $state(true);

	const getDaysInMonth = (month: number, year: number) =>
		new Date(year, month + 1, 0).getDate();

	// Convert JS Sunday=0 → Monday=0
	const getFirstDayOffset = (month: number, year: number) => {
		const day = new Date(year, month, 1).getDay();
		return day === 0 ? 6 : day - 1;
	};

	const formatDate = (year: number, month: number, day: number) =>
		`${year}-${String(month + 1).padStart(2, "0")}-${String(day).padStart(2, "0")}`;
	
	const generateCalendar = () => {
		const totalDays = getDaysInMonth(month, year);
		const offset = getFirstDayOffset(month, year);

		const prevMonth = month === 0 ? 11 : month - 1;
		const prevYear = month === 0 ? year - 1 : year;
		const prevMonthDays = getDaysInMonth(prevMonth, prevYear);

		let cells: Cell[] = [];

		// Previous month days
		for (let i = offset - 1; i >= 0; i--) {
			const day = prevMonthDays - i;
			cells.push(
				new Cell(
					formatDate(prevYear, prevMonth, day),
					day,
					prevMonth,
					prevYear,
					false,
					null,
					null,
				),
			);
		}

		// Current month days
		for (let d = 1; d <= totalDays; d++) {
			cells.push(
				new Cell(
					formatDate(year, month, d),
					d,
					month,
					year,
					true,
					null,
					null,
				),
			);
		}

		// Next month days
		const nextMonth = month === 11 ? 0 : month + 1;
		const nextYear = month === 11 ? year + 1 : year;

		while (cells.length % 7 !== 0) {
			const day = (cells.length % 7) + 1;
			cells.push(
				new Cell(
					formatDate(nextYear, nextMonth, day),
					day,
					nextMonth,
					nextYear,
					false,
					null,
					null,
				),
			);
		}

		// Split into weeks
		const weeks: Cell[][] = [];
		for (let i = 0; i < cells.length; i += 7) {
			weeks.push(cells.slice(i, i + 7));
		}
		return weeks;
	};

	const weeks = generateCalendar();

	fetch("./GetRealizationData").then((r) => {
		r.json().then((j) => {
			let realization = j;

			for (const item of realization) {
				const date = item.ActivityDate.split("T")[0];
				const cell = weeks.flat().find((x) => x.date == date);

				if (cell) {
					cell.realization = item;
				}
			}
		});
	});
	fetch("./GetPlannedData").then((r) => {
		r.json().then((j) => {
			let plans = j;
			for (const item of plans) {
				const date = item.PlanDate.split("T")[0];
				const cell = weeks.flat().find((c) => c.date === date);
				if (cell) {
					cell.plan = item;
				}
			}
			loading = false;
		});
	});

	const openModal = (cell: Cell) => {
		modalProps.showModal = true;
		modalProps.date = cell.date;
		if (cell.plan?.Id != 0 && cell.plan?.Id != null && cell.plan?.Id != undefined) {
			fetch(`./GetPlanDetails?id=${cell.plan.Id}`)
				.then((res) => res.json()
						.then((data) => {
							console.log("Fetched plan details:", data);
							modalProps.plan = data;
							console.log("Modal props after fetching details:", modalProps);
						})
						.catch((err) => {
							console.error(
								"Error parsing plan details response:",
								err,
							);
						}),
				)
				.catch((err) => {
					console.error("Error fetching plan details:", err);
				});
		} else {
			console.log("Opening modal for new plan on date:", cell.date);
		}
	};
</script>

<PlannerItemModal
	bind:showModal={modalProps.showModal}
	date={modalProps.date}
	plan={modalProps.plan}
/>

<div class="container my-5">
	<h2 class="fw-bold mb-4 text-center">
		{new Date(year, month).toLocaleString("default", { month: "long" })}
		{year}
	</h2>

	<!-- Weekday headers (Monday first) -->
	<div class="row text-center fw-bold mb-2">
		<div class="col">Mon</div>
		<div class="col">Tue</div>
		<div class="col">Wed</div>
		<div class="col">Thu</div>
		<div class="col">Fri</div>
		<div class="col">Sat</div>
		<div class="col">Sun</div>
	</div>

	<!-- Weeks -->
	{#each weeks as week}
		<div class="row mb-2">
			{#each week as cell}
				<!-- svelte-ignore a11y_click_events_have_key_events -->
				<!-- svelte-ignore a11y_no_static_element_interactions -->
				<div
					class="col border p-2"
					style="min-height:120px; cursor: pointer;"
					class:opacity-50={!cell.current}
					onclick={() => openModal(cell)}
				>
					<div class="fw-bold">{cell.day}</div>
					{#if cell.plan}
						{@const plan = cell.plan as PlannerItem}

						{#if plan?.Distance}
							<div class="small mt-1 text-primary">
								<i class="fa-solid fa-calendar-check me-1"></i>
								{formatDistance(plan.Distance)}
							</div>
						{/if}
					{/if}

					{#if cell.realization}
						{@const data = cell.realization as RealizationItem}

						{#if data?.ActivityDistance}
							<div class="small text-success">
								<i class="fa-solid fa-circle-check me-1"></i>
								{formatDistance(data.ActivityDistance)}
							</div>
						{/if}
					{/if}
				</div>
			{/each}
		</div>
	{/each}
</div>
