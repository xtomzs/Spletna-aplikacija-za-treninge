<script lang="ts">
    import type { Club } from "./models/Club";

	let clubs: Club[] = [];

    fetch(`./GetAthleteClubs`).then((r) => {
        r.json().then((j) => {
            clubs = j;            
        });
    });

	const formatMembers = (count: number) =>
		new Intl.NumberFormat().format(count);
</script>

<div class="container my-5">
	<h2 class="fw-bold mb-4">Your Clubs</h2>

	{#if clubs.length === 0}
		<div class="alert alert-info text-center">
			You are not part of any clubs yet.
		</div>
	{:else}
		<div class="row g-4">
			{#each clubs as club (club.id)}
				<div class="col-md-4">
					<div class="card shadow-2-strong h-100">

						<!-- Club Image -->
						{#if club.profile_medium}
							<img
								src="{club.profile_medium}"
								class="card-img-top"
								alt="{club.name}"                                
							/>
						{/if}

						<div class="card-body d-flex flex-column">

							<!-- Name -->
							<h5 class="fw-bold">{club.name}</h5>

							<!-- Location -->
							<p class="text-muted mb-2">
								{club.city}, {club.country}
							</p>

							<!-- Sport Type -->
							<p class="mb-2">
								<i class="fa-solid fa-person-running me-1 text-primary"></i>
								{club.sport_type}
							</p>

							<!-- Members -->
							<p class="mb-3">
								<i class="fa-solid fa-users me-1 text-success"></i>
								{formatMembers(club.member_count)} members
							</p>

							<!-- Action -->
							<a
								href="https://www.strava.com/clubs/{club.id}"
								target="_blank"
								class="btn btn-outline-primary mt-auto"
							>
								View Club
							</a>

						</div>
					</div>
				</div>
			{/each}
		</div>
	{/if}
</div>

<style>
    .card-img-top {
        
    }
</style>