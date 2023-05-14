<script>
    import { navigate } from "svelte-routing";
    import { API_URL } from "../config";
    import TokenService from "../services/TokenService";
    import { isValidEmail, isValidPassword } from "../validation/validate";
    import Menu from "./Menu.svelte";
    let email = "";
    let password = "";
    let loginAttempts = 0;
    let errorMessage = "";

    const goMain = () => {
        navigate("/");
    };

    async function loginHandle() {
        // Walidacja formularza
        if (!isValidEmail(email)) {
            errorMessage = "Nieprawidłowy format e-maila.";
            return;
        }

        if (!isValidPassword(password)) {
            errorMessage = "Nie odnaleziono.";
            return;
        }

        const userDto = {
            email: email,
            password: password,
        };

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
                TokenService.saveToken(data.data);
                goMain();
            } else {
                console.error("Error:", data.message);
                loginAttempts++;
                errorMessage = "Błąd: Nieprawidłowy e-mail lub hasło.";
            }
        } catch (error) {
            console.error("Error:", error);
        }
    }
</script>

<div class="container">
    <Menu />
    <h1>Login</h1>
    {#if errorMessage}
        <p class="error">{errorMessage}</p>
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
