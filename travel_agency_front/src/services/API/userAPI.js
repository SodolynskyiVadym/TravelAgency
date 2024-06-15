import axios from "axios";
import router from "./../router";

const mainUrl = "http://localhost:5113/auth";



export async function getUserRoleByToken(token){
    try{
        return await axios.get(`${mainUrl}/getUserRole`, {headers: {Authorization: `Bearer ${token}`}}).then((res) => res.data);
    }catch{
        router.push("/error");
    }
}


export async function registerUser(user){
    try{
        return await axios.post(`${mainUrl}/registrationUser`, user).then((res) => res.data);
    }catch(error){
        router.push("/error");
    }
}


export async function login(user){
    try{
        return await axios.post(`${mainUrl}/login`, user).then((res) => res.data);
    }catch(error){
        router.push("/error");
    }
}