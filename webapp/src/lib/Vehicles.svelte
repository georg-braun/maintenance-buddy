<script>
    import {onMount} from "svelte"
	import { getVehicles, deleteVehicle } from '../api-communication/api-vehicles';

	onMount(async () => {
		vehicles = await getVehicles();
	});

	let vehicles = [];
</script>

<div class="flex">
    {#each vehicles as vehicle}
    <div class="mr-4 mb-4">
    <div >
        <a href="/vehicles/{vehicle.id}">
            <div class="border rounded-t-md p-4">
                <p title={vehicle.id} class="text-center">{vehicle.name}</p>
                <p>{vehicle.kilometer} km</p>
            </div>
        </a>
    </div>
    <div class="bg-red-200 hover:bg-red-500 rounded-b-md text-center" on:click={async () => {

        await deleteVehicle(vehicle.id)
        vehicles = await getVehicles()

    }}>Delete</div>
</div>
    {/each}
    <div>
        <a href="/vehicles/create-vehicle">
        <div class="border text-center text-xl  h-12 w-12">
            +
        </div></a>
    </div>
</div>
