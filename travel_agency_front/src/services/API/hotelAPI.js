import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/hotel";

export async function getHotel(id){
    try {
        return await axios.get(`${mainUrl}/${id}`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }

}

export async function getAllHotels() {
    try {
        return await axios.get(`${mainUrl}/getAllHotels`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}

export async function updateHotel(hotel){
    try {
        return await axios.patch(`${mainUrl}/update/${hotel.id}`, hotel).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }

}


export async function createHotel(data) {
    try {
        return await axios.post(`${mainUrl}/create`, data).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}