import type { ApiData } from "../../../models/ApiData";

export interface DetailedActivity extends ApiData{
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
  type: string; // deprecated
  sport_type: string;
  start_date: string; // ISO date string
  start_date_local: string; // ISO date string
  timezone: string;
  achievement_count: number;
  kudos_count: number;
  comment_count: number;
  athlete_count: number;
  photo_count: number;
  map: PolylineMap;
  trainer: boolean;
  commute: boolean;
  manual: boolean;
  private: boolean;
  flagged: boolean;

  // ---- Detailed fields ----
  description?: string;
  calories?: number;
  gear?: GearSummary;
  gear_id?: string;
  average_speed: number;
  max_speed: number;

  average_heartrate?: number;
  max_heartrate?: number;
  has_heartrate?: boolean;

  average_watts?: number;
  max_watts?: number;
  weighted_average_watts?: number;
  device_watts?: boolean;
  kilojoules?: number;

  splits_metric?: SplitSummary[];
  splits_standard?: SplitSummary[];
  laps?: LapSummary[];
}
export interface MetaAthlete {
  id: number;
  resource_state: number;
}

export interface PolylineMap {
  id: string;
  resource_state: number;
  summary_polyline: string;
}

export interface SplitSummary {
  distance: number;
  elapsed_time: number;
  moving_time: number;
  average_speed: number;
  split: number;
}

export interface LapSummary {
  id: number;
  elapsed_time: number;
  moving_time: number;
  distance: number;
  average_speed: number;
}

export interface GearSummary {
  converted_distance: number;
  id: string;
  name: string;
  nickname: string;
  primary: boolean;
  resource_state: number;
  retired: boolean;
}