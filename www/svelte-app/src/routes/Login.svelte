<script>
    import { navigate } from "svelte-routing";
    import { API_URL } from "../config";
    import { isValidEmail, isValidPassword } from "../validation/validate";
    import Menu from "./Menu.svelte";
    import UserService from "../services/UserService";

    let email = "";
    let password = "";
    let errorMessage = "";

    async function loginHandle() {
        // Form validation
        if (!isValidEmail(email)) {
            errorMessage = "Nieprawidłowy format e-maila.";
            return;
        }

        if (!isValidPassword(password)) {
            errorMessage = "Nie odnaleziono.";
            return;
        }

        const userDto = { email, password };

        try {
            const response = await fetch(`${API_URL}/Users/Login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userDto),
            });

            const data = await response.json();

            if (data.success) {
                await UserService.setUser(data.data);
                navigate("/"); // Directly navigate to main
            } else {
                console.error("Error:", data.message);
                errorMessage = "Błąd: Nieprawidłowy e-mail lub hasło.";
            }
        } catch (error) {
            console.error("Error:", error);
        }
    }
</script>

<Menu />
<div class="container">
    <h1>Login</h1>
    {#if errorMessage}
        <p class="error text-danger">{errorMessage}</p>
    {/if}
    <div class="form-group">
        <label for="email">Email</label>
        <input type="text" bind:value={email} class="form-control" id="email" />
    </div>
    <div class="form-group">
        <label for="password">Password</label>
        <input
            type="password"
            bind:value={password}
            class="form-control"
            id="password"
        />
    </div>
    <button on:click={loginHandle} class="btn btn-primary">Submit</button>
</div>
