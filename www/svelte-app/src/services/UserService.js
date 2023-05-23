import { writable } from 'svelte/store';
import { navigate } from 'svelte-routing';

const USER = writable(getUser());

async function setUser(user) {
    localStorage.setItem('USER', JSON.stringify(user));
}

async function getUser() {
    const user = localStorage.getItem('USER');
    return user ? JSON.parse(user) : null;
}

function removeUser() {
    localStorage.removeItem('USER');
}

async function logout() {
    removeUser();
    setUser(null);
    navigate("/")
}

export default {
    setUser,
    getUser,
    removeUser,
    logout,
    USER
};