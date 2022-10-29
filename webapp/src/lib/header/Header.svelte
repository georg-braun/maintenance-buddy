<script lang="ts">
	import { page } from '$app/stores';
</script>

<header>
	<nav data-sveltekit-prefetch>
		<ul class="flex">
			<li class="{$page.url.pathname === '/vehicles' ? 'menu-item-active' : 'menu-item'} ">
				<a href="/vehicles">Vehicles</a>
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
		background-color: lightgray;
	}

</style>