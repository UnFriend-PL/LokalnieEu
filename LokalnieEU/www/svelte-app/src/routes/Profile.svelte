<script>
    import { onMount } from "svelte";
    import UserService from "../services/UserService";
    import { navigate } from "svelte-routing";
    import Menu from "./Menu.svelte";
    import { API_URL } from "../config";
    import validation from "../validation/validate";

    let user;
    UserService.USER.subscribe((value) => {
        user = value;
    });
    onMount(async () => {
        user = await UserService.getUser();
    });

    let repeatPassword = "";
    let password = "";
    let oldPass = "";
    let errorMessage = "";

    async function changePass() {
        if (!validation.isValidPassword(password)) {
            errorMessage =
                "Hasło musi zawierać co najmniej 8 znaków, w tym co najmniej jeden znak specjalny.";
            return;
        }
        if (password != repeatPassword) {
            errorMessage = "Hasła muszą być takie same!";
            return;
        }

        try {
            var userDto = {
                userId: user.id,
                oldPassword: oldPass,
                newPassword: password,
            };
            const response = await fetch(
                `${API_URL}/Users/UpdateUserPassword`,
                {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${user.token}`,
                    },
                    body: JSON.stringify(userDto),
                }
            );

            if (!response.ok) {
                console.log(response);
            }

            const updatedUser = await response.json();
            if (updatedUser.success) {
                await UserService.setUser(updatedUser.data);
                navigate("/");
            } else {
                console.log(updatedUser.message);
            }
        } catch (error) {
            console.log(
                "There was a problem with the fetch operation: ",
                error.message
            );
        }
    }

    async function updateUser() {
        try {
            const response = await fetch(`${API_URL}/Users/UpdateUser`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${user.token}`,
                },
                body: JSON.stringify(user),
            });

            if (!response.ok) {
                console.log(response);
            }

            const updatedUser = await response.json();
            if (updatedUser.success) {
                await UserService.setUser(updatedUser.data);
                navigate("/");
            } else {
                console.log(updatedUser.message);
            }
        } catch (error) {
            console.log(
                "There was a problem with the fetch operation: ",
                error.message
            );
        }
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

        <form on:submit|preventDefault={changePass} class="mt-5 mb-5">
            {#if errorMessage}
                <p class="error text-danger">{errorMessage}</p>
            {/if}
            <div class="form-group">
                <label for="oldPass">Stare hasło</label>
                <input
                    id="oldPass"
                    type="oldPass"
                    class="form-control"
                    bind:value={oldPass}
                />
            </div>
            <div class="form-group">
                <label for="password">Nowe hasło</label>
                <input
                    type="password"
                    id="password"
                    class="form-control"
                    bind:value={password}
                />
            </div>
            <div class="form-group">
                <label for="repeatPassword">Powtórz Nowe hasło</label>
                <input
                    type="password"
                    id="repeatPassword"
                    class="form-control"
                    bind:value={repeatPassword}
                />
            </div>
            <button type="submit" class="btn btn-primary"
                >Change password</button
            >
        </form>
        <button on:click={UserService.logout} class="btn btn-danger"
            >Logout</button
        >
    </div>
{/if}
