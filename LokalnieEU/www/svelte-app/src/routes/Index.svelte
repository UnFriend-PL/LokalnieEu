<script>
    import { link, Link } from "svelte-routing";
    import Menu from "./Menu.svelte";
    import UserService from "../services/UserService";
    import { onMount } from "svelte";

    let user;
    UserService.USER.subscribe((value) => {
        user = value;
    });
    onMount(async () => {
        user = await UserService.getUser();
        if (user !== null) {
            console.log(user.name);
        }
    });

    async function logout() {
        await UserService.logout();
        window.location.href = "/"; // Przekierowanie na stronę główną po wylogowaniu
    }
</script>

<div class="container">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <Menu />

        <a class="navbar-brand" href="/" use:link>LokalnieEU</a>
        <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target="#navbarNav"
            aria-controls="navbarNav"
            aria-expanded="false"
            aria-label="Toggle navigation"
        >
            <span class="navbar-toggler-icon" />
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ml-auto">
                {#if user === null}
                    <li class="nav-item">
                        <a class="nav-link" href="/login" use:link>Login</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/register" use:link
                            >Register</a
                        >
                    </li>
                {:else}
                    <li class="nav-item">
                        <Link class="nav-link" to="/" on:click={logout}
                            >Log out</Link
                        >
                    </li>
                {/if}
            </ul>
        </div>
    </nav>

    <div class="jumbotron">
        {#if user != null}
            <h1 class="display-1">Witaj {user.name}!</h1>
        {:else}
            <h1 class="display-1">Witaj w naszej aplikacji!</h1>
        {/if}
        <p class="lead">
            Ta aplikacja została stworzona przy użyciu Svelte i Bootstrap.
        </p>
        <hr class="my-4" />
        <p>Proszę zaloguj się lub zarejestruj, aby kontynuować.</p>
    </div>
</div>
