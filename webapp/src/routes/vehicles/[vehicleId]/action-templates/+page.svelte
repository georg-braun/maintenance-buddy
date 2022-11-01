<script lang="ts">
	import { onMount } from 'svelte';

	import {
		getActionTemplates,
		addActionTemplate,
		deleteSchedule,
		changeScheduleName,
		changeScheduleKilometerInterval,
		changeScheduleTimeInterval
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';
	import EditableField from '$lib/EditableField.svelte';
	import { goToVehicle } from '$lib/NavigationHelper';

	onMount(async () => {
		if (vehicleId !== undefined) await updateSchedules();
	});

	async function updateSchedules() {
		plans = await getActionTemplates(vehicleId);
	}
	let vehicleId = $page.params.vehicleId;

	let plans = [];

	let name = '';
	let kilometer = 0;
	let days = 0;
</script>

<section>
	<h1>Schedules</h1>

	<div class="grid grid-cols-4 auto-cols-auto gap-1">
		<div>Scheudle</div>
		<div>Kilometer</div>
		<div>Time (days)</div>
		<div />
		{#each plans as plan}
			<EditableField
				value={plan.name}
				on:value-changed={(e) => changeScheduleName(vehicleId, plan.id, e.detail.newValue)}
			/>
			<EditableField
				value={plan.kilometerInterval}
				on:value-changed={(e) =>
					changeScheduleKilometerInterval(vehicleId, plan.id, e.detail.newValue)}
			/>
			<EditableField
				value={plan.timeIntervalInDays}
				on:value-changed={(e) => changeScheduleTimeInterval(vehicleId, plan.id, e.detail.newValue)}
			/>
			<div>
				<button
					class="px-2 bg-red-300 hover:bg-red-500"
					on:click={async () => {
						await deleteSchedule(vehicleId, plan.id);
						await updateSchedules();
					}}>Delete</button
				>
			</div>
		{/each}
	</div>

	<!-- New schedule-->
	<h2 class="text-xl mt-10">Add schedule</h2>
	<div class="grid grid-cols-2 mt-2 gap-1 md:w-1/2">
		<div>Schedule</div>
		<input placeholder="Oil exchange" class=" bg-slate-50" bind:value={name} />
		<div>Kilometer interval</div>
		<input placeholder="5000" type="number" class=" bg-slate-50" bind:value={kilometer} />
		<div>Time interval in days</div>
		<input placeholder="365" type="number" class=" bg-slate-50" bind:value={days} />
		<div>
			<button
				class="rounded px-2 my-auto bg-slate-200"
				on:click={async () => {
					console.log(days);
					await addActionTemplate(vehicleId, name, kilometer, days);
					await goToVehicle(vehicleId);
				}}
				>Add
			</button>
		</div>
	</div>
</section>
