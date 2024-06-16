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

export async function updateHotel(hotel, token){
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try {
        return await axios.patch(`${mainUrl}/update/${hotel.id}`, hotel, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }

}


export async function createHotel(hotel, token) {
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try {
        return await axios.post(`${mainUrl}/create`, hotel, config).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}