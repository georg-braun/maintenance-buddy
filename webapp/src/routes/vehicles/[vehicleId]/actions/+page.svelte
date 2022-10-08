<script lang="ts">
	import { onMount } from 'svelte';
	import {
		getActions,
		addAction,
		getActionTemplates,
		deleteAction
	} from '../../../../api-communication/api-vehicles';
	import { page } from '$app/stores';

	let vehicleId = $page.params.vehicleId;

	onMount(async () => {
		if (vehicleId !== undefined) {
			actionTemplates = await getActionTemplates(vehicleId);
			actions = await getActions(vehicleId);
			selectFirstActionTemplate();
		}
	});

	let actions = [];
	let actionTemplates = [];

	// add new form
	let selectedActionTemplate;
	let date, kilometer, note;

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

	{#each actions as action}
		<div>
			{getTemplateNameById(action.actionTemplateId)}
			{new Date(action.date).toLocaleDateString()}
			{action.note}
			<button
				class="bg-red-200"
				on:click={async () => {
					console.log(action);
					await deleteAction(vehicleId, action.actionTemplateId, action.id);
				}}>Delete</button
			>
		</div>
	{/each}

	<div class="">
		<div class="my-auto">
			<select bind:value={selectedActionTemplate}>
				{#each actionTemplates as actionTemplate}
					<option value={actionTemplate}>
						{actionTemplate.name}
					</option>
				{/each}
			</select>
		</div>
		<input type="date" placeholder="Oil exchange" class="w-96 bg-slate-50" bind:value={date} />
		<input type="number" step="1" class="w-96 bg-slate-50" bind:value={kilometer} />
		<input placeholder="50W50" class="w-96 bg-slate-50" bind:value={note} />
	</div>
	<button
		on:click={async () => {
			await addAction(vehicleId, selectedActionTemplate.id, date, kilometer, note);
		}}>Add</button
	>
</section>
