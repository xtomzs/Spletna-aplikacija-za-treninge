export function formatDistance(distance: number): string {
    if (distance >= 1000) {
        return `${(distance / 1000).toFixed(2)} km`;
    } else {
        return `${distance.toFixed(0)} m`;
    }
}

export function formatElevation(elevation: number): string {
    if (elevation >= 1000) {
        return `${elevation.toFixed(2)} m`;
    } else {
        return `${elevation.toFixed(0)} m`;
    }
}