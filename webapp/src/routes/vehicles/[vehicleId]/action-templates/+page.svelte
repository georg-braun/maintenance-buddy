<script lang="ts">
	import { onMount } from 'svelte';

	import {
		getActionTemplates,
		addActionTemplate
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';
	import { LOGNAME } from '$env/static/private';

	onMount(async () => {
		if (vehicleId !== undefined)
			plans = await getActionTemplates(vehicleId);
	});

	let vehicleId = $page.params.vehicleId;

	let plans = [];

	let name = '';
	let kilometer = 0;
	let days = 0;
</script>

<section>
	<h1>Action Templates</h1>


	<div class="w-1/2 grid grid-cols-4">
		<div></div>
		<div>Kilometer</div>
		<div>Time (days)</div>
		<div></div>
		{#each plans as plan}
	
		<div title={plan.id}>{plan.name}</div>
		<div>{plan.kilometerInterval}</div>
		<div>{plan.timeIntervalInDays}</div>
		<div></div>
		{/each}

		<input placeholder="Oil exchange" class="w-96 bg-slate-50" bind:value={name} />
		<input placeholder="5000" type="number" class="w-30 bg-slate-50" bind:value={kilometer} />
		<input placeholder="365" type="number" class="w-30 bg-slate-50" bind:value={days} />
		
		
		
				<button
				class="rounded px-2 ml-4  my-auto bg-slate-200"
				on:click={async () => {
					console.log(days)
					await addActionTemplate(vehicleId, name, kilometer, days);
				}}
				>Add
			</button>
		


	
	</div>
</section>
