<script lang="ts">
	import {
		getActionTemplates,
		addActionTemplate
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';
	let vehicleId = $page.params.vehicleId;
	console.log($page.params);
	let plans = [];

	let name = '';
	export let data;
	console.log(data);
</script>

<section>
	<h1>Action Templates</h1>
	<button
		on:click={async () => {
			plans = await getActionTemplates(vehicleId);
		}}>Get</button
	>

	{#each plans as plan}
		<div>{plan.name}</div>
	{/each}

	<h1>Add a new action template</h1>
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
