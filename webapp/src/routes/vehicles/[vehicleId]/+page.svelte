<script lang="ts">
	import { onMount } from 'svelte';
	import { page } from '$app/stores';
	import { getVehicle, changeVehicleName } from '../../../api-communication/api-vehicles';

	onMount(async () => {
		await refreshVehicle();
	});

	let editVehicleName = false;

	let vehicle;
	let vehicleId = $page.params.vehicleId;
	let newVehicleName = '';

	async function refreshVehicle() {
		vehicle = undefined;
		let newVehicle = await getVehicle(vehicleId);
		vehicle = newVehicle;
		console.log(newVehicle);
	}

	async function changeName() {
		await changeVehicleName(vehicle.id, newVehicleName);

		/*
		Todo: Check why the vehicle name isn't restord. It seems that the get arrives before the change name.
		Isn't the async call awaited?
			*/
		await refreshVehicle();
		editVehicleName = false;
	}
</script>

<section>
	{#if vehicle != undefined}
		<h1>{vehicle.name}</h1>
		<div>
			<div class="w-1/2 grid grid-rows-4 grid-cols-3">
				<div>Name</div>
				<div>
					{#if editVehicleName}
						<!-- Edit name -->
						<!-- Todo: could be extracted to a component -->
						<div>
							<input bind:value={newVehicleName} class="bg-slate-100" />
							<button on:click={changeName}>💾</button>
							<button on:click={() => (editVehicleName = false)}>❌</button>
						</div>
					{:else}
						<div title={vehicle?.id}>{vehicle?.name}</div>
					{/if}
				</div>
				<div>
					<button
						title="Change vehicle name"
						on:click={() => {
							newVehicleName = vehicle?.name;
							editVehicleName = true;
						}}>✒️</button
					>
				</div>
				<div>Kilometer</div>
				<div>{vehicle?.kilometer}</div>
				<div />
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
	{/if}
</section>
