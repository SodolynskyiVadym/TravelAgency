import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/auth";



export async function getUserByToken(token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        const user = await axios.get(`${mainUrl}/getUserByToken`, config).then((res) => res.data);
        if(user) return user;
        else router.push("/login");
    } catch {
        router.push("/error");
    }
}


export async function getAllUsers(token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.get(`${mainUrl}/getAllUsers`, config).then((res) => res.data);
    } catch {
        return [];
    }
}

export async function registerUser(user) {
    try {
        return await axios.post(`${mainUrl}/registerUser`, user).then((res) => res.data);
    } catch (error) {
        return false;
    }
}

export async function createUser(user, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.post(`${mainUrl}/registerEditorAdmin`, user, config).then((res) => res.data);
    } catch (error) {
        return false;
    }
}


export async function login(user) {
    try {
        return await axios.post(`${mainUrl}/login`, user).then((res) => res.data);
    } catch (error) {
        router.push("/login");
    }
}


export async function createReservePassword(email) {
    try {
        return await axios.post(`${mainUrl}/createReservePassword?email=${email}`).then((res) => res.data);
    } catch (error) {
        router.push("/login");
    }
}


export async function loginViaReservePassword(user) {
    try {
        return await axios.post(`${mainUrl}/loginViaReservePassword`, user).then((res) => res.data);
    } catch (error) {
        router.push("/login");
    }
}


export async function updatePassword(password, token) {
    const config = { headers: { Authorization: `Bearer ${token}`, 'Content-Type': 'application/json' } }
    try {
        const data = { password: password}
        return await axios.post(`${mainUrl}/updatePassword`, data, config).then((res) => res.data);
    } catch (error) {
        return false
    }
}


export async function deleteUser(id, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.delete(`${mainUrl}/deleteUser/${id}`, config).then((res) => res.data);
    } catch {
        return false;
    }
}