<script>
    import { link } from "svelte-routing";
    import Menu from "./Menu.svelte";
</script>

<div class="container">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <Menu />

        <a class="navbar-brand" href="/" use:link>Twoja Aplikacja</a>
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
                <li class="nav-item">
                    <a class="nav-link" href="/login" use:link>Login</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/register" use:link>Register</a>
                </li>
            </ul>
        </div>
    </nav>

    <div class="jumbotron">
        <h1 class="display-1">Witaj w naszej aplikacji!</h1>
        <p class="lead">
            Ta aplikacja została stworzona przy użyciu Svelte i Bootstrap.
        </p>
        <hr class="my-4" />
        <p>Proszę zaloguj się lub zarejestruj, aby kontynuować.</p>
    </div>
</div>
