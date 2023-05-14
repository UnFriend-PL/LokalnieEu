<script>
    import { Link } from "svelte-routing";
</script>

<nav class="navbar navbar-expand-lg navbar-light bg-light fixed-top">
    <Link to="/" class="navbar-brand">Twoja Aplikacja</Link>
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
                <Link to="/" class="nav-link">Home</Link>
            </li>
            <li class="nav-item">
                <Link to="/profile" class="nav-link">Profile</Link>
            </li>
        </ul>
    </div>
</nav>
