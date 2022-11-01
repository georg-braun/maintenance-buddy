<script lang="ts">
	import { onMount } from 'svelte';
	import {
		getActions,
		addAction,
		getActionTemplates,
		deleteAction,
		changeActionNote,
		changeActionDate,
		changeActionKilometer
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';
	import EditableField from '$lib/EditableField.svelte';
	import { goToVehicle } from '$lib/NavigationHelper';

	let vehicleId = $page.params.vehicleId;

	onMount(async () => {
		await updateActions();
	});

	async function updateActions() {
		if (vehicleId !== undefined) {
			actionTemplates = await getActionTemplates(vehicleId);
			actions = await getActions(vehicleId);
			selectFirstActionTemplate();
		}
	}

	let actions = [];
	let actionTemplates = [];

	// add new form
	let selectedActionTemplate;
	let date,
		kilometer,
		note = '';

	function selectFirstActionTemplate() {
		if (actionTemplates.length > 0) selectedActionTemplate = actionTemplates[0];
	}

	function getTemplateNameById(id) {
		console.log(actionTemplates);
		let template = actionTemplates.find((_) => _.id == id);
		if (template !== undefined) return template.name;
		return id;
	}
</script>

<section>
	<h1>Actions</h1>

	<div class="grid grid-cols-5 gap-1">
		<div>Schedule</div>
		<div>Date</div>
		<div>Kilometer</div>
		<div>Note</div>
		<div />
		{#each actions as action}
			{getTemplateNameById(action.actionTemplateId)}
			<EditableField
				value={new Date(action.date)}
				type="date"
				on:value-changed={(e) =>
					changeActionDate(vehicleId, action.actionTemplateId, action.id, e.detail.newValue)}
			/>
			<EditableField
				value={action.kilometer}
				on:value-changed={(e) =>
					changeActionKilometer(vehicleId, action.actionTemplateId, action.id, e.detail.newValue)}
			/>
			<EditableField
				value={action.note}
				on:value-changed={(e) =>
					changeActionNote(vehicleId, action.actionTemplateId, action.id, e.detail.newValue)}
			/>

			<div>
				<button
					class="bg-red-200 px-2 hover:bg-red-500"
					on:click={async () => {
						console.log(action);
						await deleteAction(vehicleId, action.actionTemplateId, action.id);
						await updateActions()
					}}>Delete</button
				>
			</div>
		{/each}
	</div>

	<h1>New action</h1>
	<div class="grid grid-cols-2 gap-1 md:w-1/2">
		<div>Schedule</div>
		<div class="my-auto">
			<select bind:value={selectedActionTemplate}>
				{#each actionTemplates as actionTemplate}
					<option value={actionTemplate}>
						{actionTemplate.name}
					</option>
				{/each}
			</select>
		</div>
		<div>Day</div>
		<input type="date" placeholder="Oil exchange" class="w-96 bg-slate-50" bind:value={date} />
		<div>Kilometer</div>
		<input type="number" step="1" class="w-96 bg-slate-50" bind:value={kilometer} />
		<div>Note</div>
		<input placeholder="50W50" class="w-96 bg-slate-50" bind:value={note} />
	</div>
	<button
		class="bg-slate-100 hover:bg-slate-200 px-2"
		on:click={async () => {
			await addAction(vehicleId, selectedActionTemplate.id, date, kilometer, note);
			await updateActions();
		}}>Add</button
	>
</section>
