import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/destination";


export async function getAllDestinations() {
    try {
        return await axios.get(`${mainUrl}/getAllDestinations`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}