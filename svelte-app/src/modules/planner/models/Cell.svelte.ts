import type { PlannerItem } from "./PlannerItem";
import type { RealizationItem } from "./RealizationItem";

export class Cell implements Cell {
	constructor(
		date: string,
		day: number,
		month: number,
		year: number,
		current: boolean,
		plan: PlannerItem | null,
		realization: RealizationItem | null
	) {
		this.date = $state(date);
		this.day = $state(day);
		this.month = $state(month);
		this.year = $state(year);
		this.current = $state(current);
		this.plan = $state(plan);
		this.realization = $state(realization);
	}
}

export interface Cell {
	date: string;
	day: number;
	month: number;
	year: number;
	current: boolean;
	plan: PlannerItem | null;
	realization: RealizationItem | null;
}