import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/transport";


export async function getTransportById(id) {
    try {
        return await axios.get(`${mainUrl}/${id}`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function getAllTransports() {
    try {
        return await axios.get(`${mainUrl}/getAllTransports`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function updateTransport(id, transport, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.patch(`${mainUrl}/update/${id}`, transport, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function createTransport(transport, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.post(`${mainUrl}/create`, transport, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}