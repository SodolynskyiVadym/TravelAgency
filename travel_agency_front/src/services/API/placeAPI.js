import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/place";

export async function getAllPlaces() {
  try {
    return await axios.get(`${mainUrl}/getAllPlaces`).then((res) => res.data);
  } catch (error) {
    await router.push("/error");
  }
}

export async function getPlacesInfo() {
  try {
    return await axios.get(`${mainUrl}/getPlacesInfo`).then((res) => res.data);
  } catch (error) {
    await router.push("/error");
  }
}

export async function getPlaceById(id) {
  try {
    return await axios
      .get(`${mainUrl}/getPlaceById/${id}`)
      .then((res) => res.data);
  } catch (error) {
    await router.push("/error");
  }
}

export async function createPlace(data) {
    try {
        return await axios.post(`${mainUrl}/create`, data).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}