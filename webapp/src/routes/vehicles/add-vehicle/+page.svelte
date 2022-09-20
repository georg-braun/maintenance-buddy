<script>
	let kilometer = 1000;
	let name = '';
</script>

<div class="">
	<h1>Add a new vehicle</h1>
	<div class="">
		<input placeholder="BMW" class="w-96 bg-slate-50" bind:value={name} />
		<input step="1" class="w-32 bg-slate-50" type="number" bind:value={kilometer} />
	</div>

	
	<div class="my-auto">
		<button
			class="rounded px-2 ml-4  my-auto bg-slate-200"
			on:click={async () => {
				//await addMoneyMovement(selectedCategory.id, date, kilometer, name);
			}}
			>Add
		</button>
	</div>
</div>
