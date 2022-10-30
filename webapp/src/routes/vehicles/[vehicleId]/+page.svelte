<script lang="ts">
	import { page } from '$app/stores';
	import { getVehicle, changeVehicleName, changeVehicleKilometer } from '../../../api-communication/api-vehicles';
	import EditableField from '$lib/EditableField.svelte';
	import { afterNavigate } from '$app/navigation';

	afterNavigate(async () => {
		
        await refreshVehicle();
    })

	let vehicle;
	$: vehicleId = $page.params.vehicleId;


	async function refreshVehicle() {
		vehicle = undefined;
		let newVehicle = await getVehicle(vehicleId);
		vehicle = newVehicle;
		console.log(newVehicle);
	}

</script>

<section>
	{#if vehicle != undefined}
		<h1>{vehicle.name}</h1>
		
		<div>
			<div class="w-1/2 grid grid-rows-4 grid-cols-2">
				<div>Name</div>
				<div>
							<EditableField
					value={vehicle.name}
					on:value-changed={(e) => changeVehicleName(vehicle.id, e.detail.newValue)}
				/>
				</div>

				<div>Kilometer</div>
				<div>
					<EditableField
					value={vehicle.kilometer}
					on:value-changed={(e) => changeVehicleKilometer(vehicle.id, e.detail.newValue)}
				/>
				</div>
				<div />
			</div>
		</div>

		<h2>More:</h2>
		<ul>
			<li>
				<a href="{vehicleId}/action-templates">Schedule</a>
			</li>
			<li>
				<a href="{vehicleId}/actions">Actions</a>
			</li>
		</ul>
	{/if}
</section>
