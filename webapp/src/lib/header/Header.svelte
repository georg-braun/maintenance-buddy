<script lang="ts">
	import { page } from '$app/stores';
</script>

<header>
	<nav data-sveltekit-prefetch>
		<ul class="flex">
			<li class="{$page.url.pathname === '/vehicles' ? 'menu-item-active' : 'menu-item'} ">
				<a href="/vehicles">Vehicles</a>
			</li>
			<li class="{$page.url.pathname === '/vehicles/create-vehicle' ? 'menu-item-active' : 'menu-item'} ">
				<a href="/vehicles/create-vehicle">Add vehicle</a>
			</li>
		</ul>

	</nav>
</header>

<style>

	.menu-item{
		margin-right: 5px;
	}

	.menu-item-active{
		margin-right: 5px;
		background-color: gray;
	}

</style>