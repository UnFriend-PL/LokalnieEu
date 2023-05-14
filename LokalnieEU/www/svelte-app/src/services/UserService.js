import { API_URL } from "../config";

const USER = "user";

function setUser(user) {
    localStorage.setItem(USER, JSON.stringify(user));
    console.log("-----UserService-----");
    console.log(USER, user)
    console.log("-----UserService-----");
}

function getUser() {
    const user = localStorage.getItem(USER);
    return user ? JSON.parse(user) : null;
}

function removeUser() {
    localStorage.removeItem(USER);
}

async function updateDbUser(user) {
    console.log(user);
    try {
        console.log(`${API_URL}/UpdateUser`);
        const response = await fetch(`${API_URL}/Users/UpdateUser`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${user.token}`
            },
            body: JSON.stringify(user),
        });

        if (!response.ok) {
            console.log(response)
        }

        const updatedUser = await response.json();
        if (updatedUser.success) {
            setUser(updatedUser.data);
        } else {
            console.log(updatedUser.message);
        }
    } catch (error) {
        console.log('There was a problem with the fetch operation: ', error.message);
    }
}
export default {
    setUser,
    getUser,
    removeUser,
    updateDbUser
}