<script lang="ts">
	import { onMount } from 'svelte';
	import { getVehicles } from '../../api-communication/api-vehicles';

	onMount(async () => {
		vehicles = await getVehicles();
	});

	let vehicles = [];
</script>

<section>
	<h1>Your vehicles 🚗</h1>

	<div class="flex">
		{#each vehicles as vehicle}
		<div class="mr-4 mb-4">
			<a href="/vehicles/{vehicle.id}">
				<div class="border rounded-md p-4">
					<p title={vehicle.id} class="text-center">{vehicle.name}</p>
					<p>{vehicle.kilometer} km</p>
				</div>
			</a>
		</div>
		{/each}
	</div>
	
</section>
