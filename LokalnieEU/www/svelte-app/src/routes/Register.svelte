<script>
    import { navigate } from "svelte-routing";
    import { API_URL } from "../config";
    import {
        isValidEmail,
        isValidPassword,
        isValidPhone,
    } from "../validation/validate";
    import Menu from "./Menu.svelte";
    const go_login = () => {
        navigate("/login");
    };

    let name = "";
    let surname = "";
    let password = "";
    let email = "";
    let phone = "";
    let errorMessage = "";

    async function register() {
        if (!isValidEmail(email)) {
            errorMessage = "Nieprawidłowy format e-maila.";
            return;
        }

        if (!isValidPassword(password)) {
            errorMessage =
                "Hasło musi zawierać co najmniej 8 znaków, w tym co najmniej jeden znak specjalny.";
            return;
        }

        if (!isValidPhone(phone)) {
            errorMessage = "Nieprawidłowy format numeru telefonu.";
            return;
        }

        const userDto = {
            name: name,
            surname: surname,
            email: email,
            password: password,
            phone: phone,
        };
        try {
            const response = await fetch(`${API_URL}/Users/Register`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userDto),
            });

            const data = await response.json();

            if (data.success) {
                go_login();
            } else {
                if (data.message === "User already exists.") {
                    errorMessage =
                        "Użytkownik z tym adresem e-mail już istnieje.";
                } else {
                    console.error("Error:", data.message);
                    errorMessage =
                        "Nie udało się zarejestrować. Spróbuj ponownie.";
                }
            }
        } catch (error) {
            console.error("Error:", error);
        }
    }
</script>

<div class="container">
    <Menu />
    <h1>Register</h1>
    {#if errorMessage}
        <p class="error text-danger">{errorMessage}</p>
    {/if}
    <div class="form-group">
        <label for="name">Name</label>
        <input type="text" bind:value={name} class="form-control" id="name" />
    </div>
    <div class="form-group">
        <label for="surname">Surname</label>
        <input
            type="text"
            bind:value={surname}
            class="form-control"
            id="surname"
        />
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
    <div class="form-group">
        <label for="email">Email</label>
        <input
            type="email"
            bind:value={email}
            class="form-control"
            id="email"
        />
    </div>
    <div class="form-group">
        <label for="phone">Phone</label>
        <input type="tel" bind:value={phone} class="form-control" id="phone" />
    </div>
    <button on:click={register} class="btn btn-primary">Submit</button>
</div>
