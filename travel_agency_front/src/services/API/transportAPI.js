import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/transport";

export async function getAllTransports() {
    try {
        return await axios.get(`${mainUrl}/getAllTransports`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function createTransport(data) {
    try {
        return await axios.post(`${mainUrl}/create`, data).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}