<script>
    import { onMount } from "svelte";
    import UserService from "../services/UserService";
    import { navigate } from "svelte-routing";
    import Menu from "./Menu.svelte";

    let user;

    onMount(async () => {
        user = await UserService.getUser();
    });

    async function logout() {
        UserService.removeUser();
        navigate("/");
        // Here you can also add a redirect to login page
    }

    async function updateUser() {
        await UserService.updateDbUser(user);
        navigate("/");
    }
</script>

{#if user}
    <div class="container">
        <Menu />
        <h2>Profile</h2>
        <form on:submit|preventDefault={updateUser}>
            <div class="form-group">
                <label for="name">Imię:</label>
                <input id="name" class="form-control" bind:value={user.name} />
            </div>
            <div class="form-group">
                <label for="surname">Nazwisko:</label>
                <input
                    id="surname"
                    class="form-control"
                    bind:value={user.surname}
                />
            </div>
            <div class="form-group">
                <label for="email">Email:</label>
                <input
                    id="email"
                    class="form-control"
                    bind:value={user.email}
                />
            </div>
            <div class="form-group">
                <label for="phone">Telefon:</label>
                <input
                    id="phone"
                    class="form-control"
                    bind:value={user.phone}
                />
            </div>
            <button type="submit" class="btn btn-primary">Update profile</button
            >
        </form>

        <button on:click={logout} class="btn btn-danger">Logout</button>
    </div>
{/if}
