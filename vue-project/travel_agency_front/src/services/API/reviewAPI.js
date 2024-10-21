import axios from "axios";
import router from "../router";
import serverUrl from "@/js/serverUrl";


const mainUrl = `${serverUrl}/review`;

export async function getUserReview(tourId, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.get(`${mainUrl}/getUserReview/${tourId}`, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}


export async function getTourReviews(tourId) {
    try {
        return await axios.get(`${mainUrl}/getTourReviews/${tourId}`).then((res) => res.data);
    } catch (error) {
        return [];
    }
}


export async function createReview(review, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.post(`${mainUrl}/create`, review, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function updateReview(review, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.patch(`${mainUrl}/update`, review, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function deleteReview(tourId, token) {
    const config = { headers: { Authorization: `Bearer ${token}` } }
    try {
        return await axios.delete(`${mainUrl}/delete/${tourId}`, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

