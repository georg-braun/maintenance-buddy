<script lang="ts">
	import { onMount } from 'svelte';

	import {
		getActionTemplates,
		addActionTemplate
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';

	onMount(async () => {
		if (vehicleId !== undefined)
			plans = await getActionTemplates(vehicleId);
	});

	let vehicleId = $page.params.vehicleId;

	let plans = [];

	let name = '';
</script>

<section>
	<h1>Action Templates</h1>

	{#each plans as plan}
		<div>{plan.Name} ({plan.Id})</div>
	{/each}

	<div class="">
		<input placeholder="Oil exchange" class="w-96 bg-slate-50" bind:value={name} />
	</div>

	<div class="my-auto">
		<button
			class="rounded px-2 ml-4  my-auto bg-slate-200"
			on:click={async () => {
				await addActionTemplate(vehicleId, name, 0, 0);
			}}
			>Add
		</button>
	</div>
</section>
