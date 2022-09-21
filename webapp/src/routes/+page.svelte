<script lang="ts">
	import Counter from '$lib/Counter.svelte';
	import Header from '$lib/header/Header.svelte';
	import { getVehicles } from '../api-communication/api-vehicles';

	let vehicles = [];
</script>

<svelte:head>
	<title>Home</title>
	<meta name="description" content="Svelte demo app" />
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
		<div>{vehicle.name} {vehicle.kilometer} km</div>
	{/each}
</section>

<style>
	section {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		flex: 1;
	}

	h1 {
		width: 100%;
	}

	.welcome {
		display: block;
		position: relative;
		width: 100%;
		height: 0;
		padding: 0 0 calc(100% * 495 / 2048) 0;
	}

	.welcome img {
		position: absolute;
		width: 100%;
		height: 100%;
		top: 0;
		display: block;
	}
</style>
