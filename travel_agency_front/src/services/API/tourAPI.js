import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/tour";


export async function getTourById(id) {
    try {
        return await axios.get(`${mainUrl}/${id}`).then((res) => res.data);
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

export async function getAllToursForeignKeys() {
    try {
        return await axios.get(`${mainUrl}/getToursForeignKeys`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function updateTour(id, tour, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.patch(`${mainUrl}/update/${id}`, tour, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function createTour(tour, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.post(`${mainUrl}/create`, tour, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}