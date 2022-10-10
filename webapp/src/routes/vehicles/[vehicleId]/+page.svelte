<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/stores';
	import { getVehicle } from '../../../api-communication/api-vehicles';

	onMount(async () => {
		vehicle = await getVehicle(vehicleId);
		console.log(vehicle);
	});

	let vehicle;
	let vehicleId = $page.params.vehicleId;
</script>

<section>
	<h1>{vehicle?.name}</h1>
	<div>
		<div class="w-1/2 grid grid-rows-4 grid-cols-2">
			<div>Name</div>
			<div title={vehicle?.id}>{vehicle?.name}</div>
			<div>Kilometer</div>
			<div>{vehicle?.kilometer}</div>
		</div>
	</div>

	<h2>Actions</h2>
	<ul>
		<li>
			<a href="{vehicleId}/action-templates">Templates</a>
		</li>
		<li>
			<a href="{vehicleId}/actions">History</a>
		</li>
	</ul>
</section>
