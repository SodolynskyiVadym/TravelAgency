import axios from "axios";
import router from "./../router";
import serverUrl from "@/js/serverUrl";


const mainUrl = `${serverUrl}/tour`;

export async function getTourById(id) {
    try {
        const tour = await axios.get(`${mainUrl}/${id}`).then((res) => res.data);
        if (tour) return tour;
        else await router.push("/");
    } catch (error) {
        await router.push("/");
    }
}

export async function getAllTours() {
    try {
        console.log("mainUrl", mainUrl);
        return await axios.get(`${mainUrl}/getAllTours`).then((res) => res.data);
    } catch (error) {
        return [];
    }
}


export async function getAvailableTours(){
    try {
        console.log("mainUrl", mainUrl);
        const data = await axios.get(`${mainUrl}/getAvailableTours`).then((res) => res.data);
        console.log("res.data", data);
        return data;
    } catch (error) {
        console.log("error", error);
        return [];
    }
}


export async function getUnavailableTours(token){
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.get(`${mainUrl}/getUnavailableTours`, config).then((res) => res.data);
    } catch (error) {
        return [];
    }

}

export async function getAllToursForeignKeys() {
    try {
        return await axios.get(`${mainUrl}/getToursForeignKeys`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function updateTour(tour, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.patch(`${mainUrl}/update`, tour, config).then((res) => res.data);
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