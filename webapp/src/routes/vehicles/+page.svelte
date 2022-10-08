<script lang="ts">
	import { onMount } from 'svelte';
	import { getVehicles } from '../../api-communication/api-vehicles';

	onMount(async () => {
		vehicles = await getVehicles();
	});

	let vehicles = [];
</script>


<section>
	<h1>Vehicles</h1>

	{#each vehicles as vehicle}
	<a href="/vehicles/{vehicle.Id}">
		<div>
			{vehicle.Name}
			{vehicle.Kilometer} km
		</div>
	</a>
	{/each}
</section>
