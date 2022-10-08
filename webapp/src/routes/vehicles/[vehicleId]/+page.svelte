<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/stores';
	import { getVehicleSummary } from '../../../api-communication/api-vehicles';


	onMount(async () => {
		vehicle = await getVehicleSummary(vehicleId);
		console.log(vehicle)
	});

	let vehicle;
	let vehicleId = $page.params.vehicleId;

</script>


<section>
	<div>
		{vehicle?.Name}
		{vehicle?.Kilometer}
		({vehicle?.Id})
	</div>

	<a href="{vehicleId}/action-templates">Action templates</a>
	<a href="{vehicleId}/actions">Actions</a>
</section>
