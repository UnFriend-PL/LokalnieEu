export const isValidEmail = (email) => {
    const regex = /\S+@\S+\.\S+/;
    return regex.test(email);
};

export const isValidPassword = (password) => {
    const regex = /^(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/;
    return regex.test(password);
};

export const isValidPhone = (phone) => {
    // Prosty regex do walidacji telefonu
    // Dostosuj do własnych potrzeb
    const regex = /^\d{9}$/;
    return regex.test(phone);
};

export default {
    isValidEmail,
    isValidPassword,
    isValidPhone
}