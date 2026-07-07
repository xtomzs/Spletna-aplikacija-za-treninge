import type { PlannerItem } from "./PlannerItem";

export class PlannerItemModalModel{
    showModal: boolean;
    date: string;
    plan: PlannerItem | null;
    constructor(
        showModal: boolean,
        date: string,
        plan: PlannerItem | null
    ) {
        this.showModal = $state(showModal);
        this.date = $state(date);
        this.plan = $state(plan);
    }
}