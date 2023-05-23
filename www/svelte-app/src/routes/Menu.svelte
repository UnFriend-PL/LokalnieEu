<script>
    import { link, Link } from "svelte-routing";
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
        window.location.href = "/"; // Redirect to the homepage after logout
    }
</script>

<footer>
    <div class="footer-menu">
        <Link to="/index">Home</Link>

        {#if user}
            <Link to="/profile">Profile</Link>
            <button on:click={logout}>Logout</button>
        {:else}
            <Link to="/login">Login</Link>
            <Link to="/register">Register</Link>
        {/if}
    </div>
</footer>

<style>
    .footer-menu {
        position: fixed;
        bottom: 0;
        width: 100%;
        background-color: #f8f9fa;
        padding: 10px 0;
        text-align: center;
    }
</style>
