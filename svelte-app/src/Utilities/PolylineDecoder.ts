/**
 * Decodes a Google-encoded polyline string into an array of lat/lng coordinates
 * @param encoded - The encoded polyline string
 * @returns Array of {lat, lng} objects
 */
export function decodePolyline(encoded: string): Array<{ lat: number; lng: number }> {
	const points: Array<{ lat: number; lng: number }> = [];
	let index = 0;
	let lat = 0;
	let lng = 0;

	while (index < encoded.length) {
		let result = 0;
		let shift = 0;
		let byte;

		// Decode latitude
		do {
			byte = encoded.charCodeAt(index++) - 63;
			result |= (byte & 0x1f) << shift;
			shift += 5;
		} while (byte >= 0x20);

		let dlat = (result & 1) ? ~(result >> 1) : result >> 1;
		lat += dlat;

		result = 0;
		shift = 0;

		// Decode longitude
		do {
			byte = encoded.charCodeAt(index++) - 63;
			result |= (byte & 0x1f) << shift;
			shift += 5;
		} while (byte >= 0x20);

		let dlng = (result & 1) ? ~(result >> 1) : result >> 1;
		lng += dlng;

		points.push({
			lat: lat / 1e5,
			lng: lng / 1e5
		});
	}

	return points;
}
