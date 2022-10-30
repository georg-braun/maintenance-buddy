<script>
	import { getPendingActions } from '../api-communication/api-vehicles';
	import { onMount } from 'svelte';

	export let vehicleId;

	let pendingActions = [];

	onMount(async () => {
		console.log(`Get pending actions for ${vehicleId}`);
		pendingActions = await getPendingActions(vehicleId) ?? [];
		console.log(pendingActions);
	});
</script>

<div class="w-1/2 grid grid-cols-3">
    <div>Schedule</div>
    <div>Time</div>
    <div>Kilometer</div>
{#each pendingActions as pendingAction}
<div>
    {pendingAction.name}
</div>
<div>
    {pendingAction.timeTillAction}
</div>
<div>
    {pendingAction.kilometerTillAction}	
</div>
{/each}
</div>