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
		<div>
			{vehicle.name}
			{vehicle.kilometer} km
			<a href="vehicles/{vehicle.id}/action-templates">Action templates</a>
			<a href="vehicles/{vehicle.id}/actions">Actions</a>
		</div>
	{/each}
</section>
