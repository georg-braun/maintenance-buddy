<script lang="ts">
	import type { PageData } from './$types';

	import { getVehicles } from '../../api-communication/api-vehicles';

	let vehicles = [];

	export let data: PageData;
	$: todos = data.todos;
</script>

<svelte:head>
	<title>Vehicles</title>
	<meta name="description" content="A todo list app" />
</svelte:head>

<section>
	<h1>Vehicles</h1>
	<button
		on:click={async () => {
			vehicles = await getVehicles();
		}}>Get</button
	>

	<div>Vehicles:</div>
	{#each vehicles as vehicle}
		<div>
			{vehicle.name}
			{vehicle.kilometer} km
			<a href="vehicles/{vehicle.id}/action-templates">Action templates</a>
		</div>
	{/each}
</section>
