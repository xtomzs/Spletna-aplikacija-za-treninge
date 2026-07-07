<script lang="ts">
    import type { DetailedActivity } from "./models/DetailedActivity";
    import ActivityMap from "./ActivityMap.svelte";

    const id = new URLSearchParams(window.location.search).get("id");
    var activity = <DetailedActivity>{};
    fetch(`./GetActivityDetails?id=${id}`).then((r) => {
        r.json().then((j) => {
            activity = j;
            console.log("Fetched activity details:", activity);
            activity.loaded = true;            
        });
    });
    // Helpers
    const formatDistance = (m: number) =>
        new Intl.NumberFormat().format(m / 1000) + " km";

    const formatTime = (seconds: number) =>
        new Date(seconds * 1000).toISOString().slice(11, 19);

    const formatDate = (date: string) => new Date(date).toDateString();

    const toPace = (speed: number) => {
        if (speed <= 0) return "Invalid speed";
        
        const totalSeconds = 1000 / speed;
        const minutes = Math.floor(totalSeconds / 60);
        const seconds = Math.round(totalSeconds % 60);
        const formattedSeconds = seconds.toString().padStart(2, "0");

        return `${minutes}:${formattedSeconds} min/km`;
    };
</script>

<div class="container my-5">
    {#if !activity.loaded}
        <p>Loading activity details...</p>
    {:else}
        <div class="card shadow-2-strong">
            <div class="card-body">
                <!-- Header -->
                <div class="mb-4 text-center">
                    <h2 class="fw-bold">{activity.name}</h2>
                    <p class="text-muted mb-0">
                        {formatDate(activity.start_date)}
                    </p>
                </div>

                <!-- Summary -->
                <section class="row g-4 text-center mb-4">
                    <div class="col-md-3">
                        <i class="fa-solid fa-route fa-2x text-primary mb-2"
                        ></i>
                        <h6 class="fw-bold">Distance</h6>
                        <p class="text-muted">
                            {formatDistance(activity.distance)}
                        </p>
                    </div>

                    <div class="col-md-3">
                        <i class="fa-solid fa-stopwatch fa-2x text-success mb-2"
                        ></i>
                        <h6 class="fw-bold">Moving Time</h6>
                        <p class="text-muted">
                            {formatTime(activity.moving_time)}
                        </p>
                    </div>

                    <div class="col-md-3">
                        <i class="fa-solid fa-mountain fa-2x text-warning mb-2"
                        ></i>
                        <h6 class="fw-bold">Elevation</h6>
                        <p class="text-muted">
                            {activity.total_elevation_gain} m
                        </p>
                    </div>

                    <div class="col-md-3">
                        <i
                            class="fa-solid fa-person-running fa-2x text-danger mb-2"
                        ></i>
                        <h6 class="fw-bold">Type</h6>
                        <p class="text-muted">{activity.type}</p>
                    </div>
                </section>

                <!-- Extra Info -->
                <section class="mt-4">
                    <h5 class="fw-bold mb-3">Additional Info</h5>
                    <ul class="list-group list-group-light">
                        <li class="list-group-item">
                            <strong>Average Pace:</strong>                            
                            {activity.average_speed
                                ? toPace(activity.average_speed)
                                : "N/A"}
                        </li>                        
                        <li class="list-group-item">
                            <strong>Kudos:</strong>
                            {activity.kudos_count}
                        </li>
                        <li class="list-group-item">
                            <strong>Average Heart Rate:</strong>
                            {activity.average_heartrate
                                ? activity.average_heartrate.toFixed(1) + " bpm"
                                : "N/A"}
                        </li>
                         <li class="list-group-item">
                            <strong>Shoe:</strong>
                            {activity.gear?.name}                                                                
                        </li>
                    </ul>
                </section>                
            </div>
        </div>

        <!-- Activity Map -->
        <div class="card shadow-2-strong mt-4">
            <div class="card-body">
                <h5 class="fw-bold mb-3">Route Map</h5>
                <ActivityMap mapData={activity.map} />
            </div>
        </div>
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