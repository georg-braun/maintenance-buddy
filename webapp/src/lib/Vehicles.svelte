<script>
	import { onMount } from 'svelte';
    import { page } from '$app/stores';
	import { getVehicles, deleteVehicle } from '../api-communication/api-vehicles';
	import { afterNavigate } from '$app/navigation';

	onMount(async () => {
        console.log("vehicle mount")
		vehicles = await getVehicles();
	});

    afterNavigate(async () => {
        console.log("after navigate vehicles")
        vehicles = await getVehicles();
    })

    $: {
        console.log($page.params)
    }


    $: pageVehicleId = $page.params.vehicleId


	let vehicles = [];
</script>

<div class="flex flex-wrap">
	{#each vehicles as vehicle}
		<div class="mr-4 mb-4 flex-none">
            <a href="/vehicles/{vehicle.id}">
			<div >
				<div class="border rounded-t-md p-4 {pageVehicleId == vehicle.id ? `` : `bg-slate-200`}">
					<p title={vehicle.id} class="text-center">{vehicle.name}</p>
					<p>{vehicle.kilometer} km</p>
				</div>
			</div>
            </a>
			<div
				class="bg-red-200 hover:bg-red-500 rounded-b-md text-center"
				on:click={async () => {
					await deleteVehicle(vehicle.id);
					vehicles = await getVehicles();
				}}
			>
				Delete
			</div>
		</div>
	{/each}
	<div>
		<a href="/vehicles/create-vehicle">
			<div class="border text-center text-xl  w-12">+</div></a
		>
	</div>
</div>
