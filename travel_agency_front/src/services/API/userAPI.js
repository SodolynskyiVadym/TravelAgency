import axios from "axios";
// import router from "./../router";

const mainUrl = "http://localhost:5113/auth";



export async function getUserByToken(token){
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try{
        return await axios.get(`${mainUrl}/getUserByToken`, config).then((res) => res.data);
    }catch{
        // router.push("/error");
    }
}


export async function getAllUsers(token){
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try{
        return await axios.get(`${mainUrl}/getAllUsers`, config).then((res) => res.data);
    }catch{
        // router.push("/error");
    }
}

export async function registerUser(user){
    try{
        return await axios.post(`${mainUrl}/registerUser`, user).then((res) => res.data);
    }catch(error){
        // router.push("/error");
    }
}

export async function createUser(user, token){
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try{
        return await axios.post(`${mainUrl}/createUser`, user, config).then((res) => res.data);
    }catch(error){
        // router.push("/error");
    }
}


export async function login(user){
    try{
        return await axios.post(`${mainUrl}/login`, user).then((res) => res.data);
    }catch(error){
        // router.push("/error");
    }
}

export async function updatePassword(password, token){
    const config = {headers: {Authorization: `Bearer ${token}`, 'Content-Type': 'application/json',}}
    try{
        return await axios.post(`${mainUrl}/updatePassword`, password, config).then((res) => res.data);
    }catch(error){
        console.log(error);
        // router.push("/error");
    }
}

export async function deleteUser(id, token){
    const config = {headers: {Authorization: `Bearer ${token}`}}
    try{
        return await axios.delete(`${mainUrl}/deleteUser/${id}`, config).then((res) => res.data);
    }catch{
        // router.push("/error");
    }
}



