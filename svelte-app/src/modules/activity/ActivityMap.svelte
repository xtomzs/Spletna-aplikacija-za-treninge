<script lang="ts">
	import { decodePolyline } from "../../Utilities/PolylineDecoder";
	import type { PolylineMap } from "../dashboard/models/PolylineMap";

	interface Props {
		mapData?: PolylineMap;
	}

	let { mapData } = $props();

	let mapContainer: HTMLDivElement;
	let map: any;
	let polylineObj: any;

	$effect(() => {
		if (!mapData || !mapContainer) return;

		// Load Google Maps API if not already loaded
		if (!(window as any).google) {
			loadGoogleMapsAPI();
		} else {
			initializeMap();
		}
	});

	function loadGoogleMapsAPI() {
		const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || "YOUR_API_KEY_HERE";
		const script = document.createElement("script");
		script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}`;
		script.async = true;
		script.defer = true;
		script.onload = initializeMap;
		document.head.appendChild(script);
	}

	function initializeMap() {
		if (!mapData || !mapContainer) return;

		const google = (window as any).google;
		if (!google) {
			console.error("Google Maps API not loaded");
			return;
		}

		// Decode the polyline to get coordinates
		const points = decodePolyline(mapData.summary_polyline);

		if (points.length === 0) {
			console.error("No points decoded from polyline");
			return;
		}

		// Calculate center of the route
		const center = calculateCenter(points);

		// Create map
		map = new google.maps.Map(mapContainer, {
			zoom: 13,
			center: center,
			mapTypeId: "terrain",
			fullscreenControl: true,
			mapTypeControl: true,
			zoomControl: true,
		});

		// Draw polyline on map
		polylineObj = new google.maps.Polyline({
			path: points,
			geodesic: true,
			strokeColor: "#FF0000",
			strokeOpacity: 0.8,
			strokeWeight: 3,
			map: map,
		});

		// Add markers for start and end points
		new google.maps.Marker({
			position: points[0],
			map: map,
			title: "Start",
			icon: "http://maps.google.com/mapfiles/ms/icons/green-dot.png",
		});

		new google.maps.Marker({
			position: points[points.length - 1],
			map: map,
			title: "End",
			icon: "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
		});

		// Fit map to polyline bounds
		const bounds = new google.maps.LatLngBounds();
		points.forEach((point) => {
			bounds.extend(new google.maps.LatLng(point.lat, point.lng));
		});
		map.fitBounds(bounds);
	}

	function calculateCenter(
		points: Array<{ lat: number; lng: number }>
	): any {
		if (points.length === 0) return { lat: 0, lng: 0 };

		const sum = points.reduce(
			(acc, point) => ({
				lat: acc.lat + point.lat,
				lng: acc.lng + point.lng,
			}),
			{ lat: 0, lng: 0 }
		);

		return {
			lat: sum.lat / points.length,
			lng: sum.lng / points.length,
		};
	}
</script>

<div class="activity-map-container" style="width: 100%;
		margin: 2rem 0;">
	{#if !mapData}
		<div class="map-placeholder" style="width: 100%;
		height: 500px;
		display: flex;
		align-items: center;
		justify-content: center;
		background: #f5f5f5;
		border-radius: 8px;
		color: #999;
		font-size: 1.1rem;">
			<p>No map data available</p>
		</div>
	{:else}
		<div bind:this={mapContainer} class="google-map" style="width: 100%;
		height: 500px;
		border-radius: 8px;
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
		overflow: hidden;"></div>
	{/if}
</div>

<style>
	.activity-map-container {
		width: 100%;
		margin: 2rem 0;
	}

	.google-map {
		width: 100%;
		height: 500px;
		border-radius: 8px;
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
		overflow: hidden;
	}

	.map-placeholder {
		width: 100%;
		height: 500px;
		display: flex;
		align-items: center;
		justify-content: center;
		background: #f5f5f5;
		border-radius: 8px;
		color: #999;
		font-size: 1.1rem;
	}
</style>
