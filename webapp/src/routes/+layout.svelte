<script lang="ts">
	import Header from '$lib/header/Header.svelte';
	import { env } from '$env/dynamic/public';
	import '../app.css';
	import { onMount } from 'svelte';
	import auth from '../auth-service';
	import Vehicles from '$lib/Vehicles.svelte';

	onMount(async () => {
		console.log(env?.PUBLIC_DEV_MODE);
		if (env?.PUBLIC_DEV_MODE != 'active') await auth.createClient();
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
	<div class="py-2 text-center bg-gradient-to-r from-slate-400 to-white">
		<div class="mx-auto text-2xl">Maintenance buddy</div>
		<div class="absolute top-2 right-2 ">
			{#if $isAuthenticated}
				<span>{$user.name}</span>
				<a href="/#" on:click={logout}>Log Out</a>
			{:else}
				<a href="/#" on:click={login}>Log In</a>
			
			{/if}
		</div>
	</div>
	<div class="flex flex-wrap">
		{#if $isAuthenticated}
			<Header />
		{/if}
		<div class="ml-auto" />
	</div>
	{#if !$isAuthenticated}
	<div class="mt-10 text-center">
		<a href="/#" on:click={login}>Log In</a>
	</div>
	{/if}
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
