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
		<a href="/vehicles/{vehicle.id}">
			<div>
				{vehicle.name}
				{vehicle.kilometer} km
			</div>
		</a>
	{/each}
</section>
