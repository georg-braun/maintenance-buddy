<script>
	import { createEventDispatcher } from 'svelte';
	const dispatch = createEventDispatcher();

	export let value = 'some value';
	export let type = 'text';
	let changedValue = '';
	let editMode = false;

	async function changeValue() {
		dispatch('value-changed', {
			newValue: changedValue
		});

		editMode = false;
	}
</script>

<div>
	<div>
		{#if editMode}
			<!-- Edit name -->
			<div class="flex">
				{#if type == 'date'}
					<input bind:value={changedValue} type="date" class="bg-slate-100 w-40" />
				{:else}
					<input bind:value={changedValue} class="bg-slate-100 w-40" />
				{/if}
				<button on:click={changeValue}>💾</button>
				<button on:click={() => (editMode = false)}>❌</button>
			</div>
		{:else}
			<div class="flex">
				<div class="w-40">
					{#if type == 'date'}
						{value.toLocaleDateString()}
					{:else}
						{value}
					{/if}
				</div>
				<button
					title="Change value"
					on:click={() => {
						changedValue = value;
						editMode = true;
					}}
				>
					✒️
				</button>
			</div>
		{/if}
	</div>
</div>
