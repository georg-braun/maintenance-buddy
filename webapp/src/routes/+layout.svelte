<script lang="ts">
	import Header from '$lib/header/Header.svelte';
	import { env } from '$env/dynamic/public';
	import '../app.css';
	import { onMount } from 'svelte';
	import auth from '../auth-service';
	import Vehicles from '$lib/Vehicles.svelte';
	

	onMount(async () => {
		console.log(env?.PUBLIC_DEV_MODE);
		if (env?.PUBLIC_DEV_MODE != "active")
			await auth.createClient();
	});

	function login() {
		auth.loginWithPopup();
	}

	function logout() {
		auth.logout();
	}




	let { isAuthenticated, user } = auth;
	// just for developing purposes to ignore the login
</script>

<main>

	<div class="flex flex-wrap">
		{#if $isAuthenticated}
			<Header />
		{/if}
		<div class="ml-auto">
		{#if $isAuthenticated}
		<span>{$user.name}</span>
		<a href="/#" on:click={logout}>Log Out</a>
		{:else}
			<a href="/#" on:click={login}>Log In</a>
			<span>Not logged in</span>
		{/if}
		</div>
	</div>
	{#if $isAuthenticated}
	<div class="mt-4">
		<Vehicles />
		<slot />
	</div>
	{/if}
</main>

<footer>
	<p>welcome to the maintenance app ({env?.PUBLIC_SYSTEM_IDENTIFIER})</p>
</footer>

<style>
	main {
		flex: 1;
		display: flex;
		flex-direction: column;
		padding: 1rem;
		width: 100%;
		max-width: 1024px;
		margin: 0 auto;
		box-sizing: border-box;
	}

	footer {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 40px;
	}

	footer a {
		font-weight: bold;
	}

	@media (min-width: 480px) {
		footer {
			padding: 40px 0;
		}
	}
</style>
