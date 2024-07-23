import axios from "axios";
import router from "./../router";
import serverUrl from "@/js/serverUrl";

const mainUrl = `${serverUrl}/destination`;


export async function getAllDestinations() {
    try {
        return await axios.get(`${mainUrl}/getAllDestinations`).then((res) => res.data);
    } catch (error) {
        await router.push("/error");
    }
}