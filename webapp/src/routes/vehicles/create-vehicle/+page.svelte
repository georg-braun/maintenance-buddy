<script>
	import { goToAllVehicles } from '$lib/NavigationHelper';
	import { createVehicle } from '../../../api-communication/api-vehicles';

	let kilometer = 1000;
	let name = '';
</script>

<div class="">
	<h1>Add a new vehicle</h1>
	
	<div class="grid grid-cols-2 w-1/2 gap-1">
		<div>Name</div>
		<input placeholder="BMW" class="w-32 bg-slate-50" bind:value={name} />
		<div>Kilometer</div>
		<input step="1" class="w-32 bg-slate-50" type="number" bind:value={kilometer} />
	</div>

	<div class="my-auto">
		<button
			class="rounded px-2 my-auto bg-slate-200"
			on:click={async () => {
				await createVehicle(name, kilometer);
				await goToAllVehicles();
			}}
			>Add
		</button>
	</div>
</div>
