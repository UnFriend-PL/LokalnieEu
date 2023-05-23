<script>
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
</script>

<Menu />
<div class="container">
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
