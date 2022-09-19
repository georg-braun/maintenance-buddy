<script lang="ts">
	import Header from '$lib/header/Header.svelte';
	import '../app.css';
	import { onMount } from 'svelte';
	import auth from '../auth-service';



	onMount(async () => {
		console.log('Mounting app');
		await auth.createClient();
	});

	function login() {
		auth.loginWithPopup();
	}

	function logout() {
		auth.logout();
	}

	let { isAuthenticated, user } = auth;
</script>

<Header />

<main>
	<div class="my-auto ml-2">
		{#if $isAuthenticated}
			<span>{$user.name} ({$user.email})</span>
		{:else}<span>Not logged in</span>{/if}

		<span>
			{#if $isAuthenticated}
				<a href="/#" on:click={logout}>Log Out</a>
			{:else}
				<a href="/#" on:click={login}>Log In</a>
			{/if}
		</span>		
	</div>

	<slot />
</main>

<footer>
	<p>visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to learn SvelteKit</p>
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
