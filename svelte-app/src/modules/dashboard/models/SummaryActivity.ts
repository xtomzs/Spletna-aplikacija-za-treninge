import type { MetaAthlete } from "./MetaAthlete";
import type { PolylineMap } from "./PolylineMap";


export interface SummaryActivity {
    id: number;
    external_id: string;
    upload_id: number;
    athlete: MetaAthlete;
    name: string;
    distance: number;
    moving_time: number;
    elapsed_time: number;
    total_elevation_gain: number;
    elev_high: number;
    elev_low: number;
    type: string;
    sport_type: string;
    start_date: Date;
    start_date_local: Date;
    timezone: string;
    achievement_count: number;
    kudos_count: number;
    comment_count: number;
    athlete_count: number;
    photo_count: number;
    total_photo_count: number;
    map: PolylineMap;
    trainer: boolean;
    commute: boolean;
    manual: boolean;
    private: boolean;
    flagged: boolean;
    workout_type: number;
    upload_id_str: string;
    average_speed: number;
    max_speed: number;
    has_kudoed: boolean;
    hide_from_home: boolean;
    gear_id: string;
    kilojoules: number;
    average_watts: number;
    device_watts: boolean;
    max_watts: number;
    weighted_average_watts: number;
}

