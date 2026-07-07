import type { ApiData } from "../../../models/ApiData";

export interface Club extends ApiData {
  id: number;
  resource_state: number; // 1 = meta, 2 = summary, 3 = detail
  name: string;
  profile_medium: string;
  cover_photo: string;
  cover_photo_small: string;

  // Deprecated
  sport_type?: string;

  // Preferred
  activity_types: ActivityType[];

  city: string;
  state: string;
  country: string;

  private: boolean;
  member_count: number;
  featured: boolean;
  verified: boolean;

  url: string;
}

export type ActivityType =
  | "cycling"
  | "running"
  | "triathlon"
  | "other";