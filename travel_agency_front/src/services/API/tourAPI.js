import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/tour";


export async function getTourById(id) {
    try {
        return await axios.get(`${mainUrl}/getTour/${id}`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function getAllTours() {
    try {
        return await axios.get(`${mainUrl}/getAllTours`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function createTour(data) {
    try {
        return await axios.post(`${mainUrl}/create`, data).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}