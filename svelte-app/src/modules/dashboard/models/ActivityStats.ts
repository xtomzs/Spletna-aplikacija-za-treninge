import type { ApiData } from "../../../models/ApiData";
import type { ActivityTotal } from "./ActivityTotal";

export interface ActivityStats extends ApiData {
    biggest_ride_distance: number;
    biggest_climb_elevation_gain: number;
    recent_ride_totals: ActivityTotal;
    recent_run_totals: ActivityTotal;
    recent_swim_totals: ActivityTotal;
    ytd_ride_totals: ActivityTotal;
    ytd_run_totals: ActivityTotal;
    ytd_swim_totals: ActivityTotal;
    all_ride_totals: ActivityTotal;
    all_run_totals: ActivityTotal;
    all_swim_totals: ActivityTotal;
}
